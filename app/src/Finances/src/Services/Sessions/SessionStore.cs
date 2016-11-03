using System;
using System.Threading.Tasks;
using Finances.Data;
using Finances.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Finances.Services.Sessions {
    public class SessionStore : ISessionStore {

        private readonly ApplicationDbContext _database;

        private readonly UserManager<ApplicationUser> _userManager; 

        private ILogger _logger;

        private readonly TimeSpan _sessionDuration;

        private readonly TimeSpan _persistentSessionDuration;

        public SessionStore(ApplicationDbContext database, UserManager<ApplicationUser> userManager, ILogger<SessionStore> logger) {
            _database = database;
            _logger = logger;
            _userManager = userManager;

            _sessionDuration = TimeSpan.FromHours(1);
            _persistentSessionDuration = TimeSpan.FromHours(24);
        }

        public Task<Session> GetSessionById(string id) {
            if (string.IsNullOrWhiteSpace(id)) {
                throw new ArgumentException("Cannot find session: session id cannot be empty or null", nameof(id));
            }

            return _database.Sessions.GetActualById(id);
        }

        public async Task<Session> CreateSessionForUser(string userName, bool persistent) {
            if (string.IsNullOrWhiteSpace(userName)) {
                throw new ArgumentException("Cannot create session: user name cannot be empty or null", nameof(userName));
            }

            using (var transaction = await _database.Database.BeginTransactionAsync()) {
                var user = await _userManager.FindByNameAsync(userName);

                if (user == null) {
                    throw new ArgumentException($"Cannot create session: user '${userName}' not found", nameof(userName));
                }

                var newSessionId = Guid.NewGuid();
                while (await _database.Sessions.GetById(newSessionId) != null) {
                    newSessionId = Guid.NewGuid();
                }

                var createdAt = DateTime.Now;
                var duration = persistent ? _persistentSessionDuration : _sessionDuration;
                var expiresAt = createdAt + duration;

                var session = new Session() {
                    Id = newSessionId,
                    CreatedAt = DateTime.Now,
                    ExpiresAt = expiresAt,
                    User = user
                };

                _database.Sessions.Add(session);
                await _database.SaveChangesAsync();

                transaction.Commit();

                _logger.LogInformation($"Session for ${userName} has been created");

                return session;
            }
        }

        public async Task CloseSession(string sessionId) {
            if (string.IsNullOrWhiteSpace(sessionId)) {
                throw new ArgumentException("Cannot find session: session id cannot be empty or null", nameof(sessionId));
            }

            var session = await _database.Sessions.GetActualById(sessionId);

            session.ClosedAt = DateTime.Now;

            await _database.SaveChangesAsync();
        }
    }
}