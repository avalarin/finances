using Finances.Models;

namespace Finances.Services.Units {
    public class CreateUnitResult : OperationResult<CreateUnitErrorCode, Unit> {

        public Unit Unit { get; }

        protected override Unit Payload => Unit;

        public CreateUnitResult(CreateUnitErrorCode? errorCode) : base(errorCode) {
        }

        public CreateUnitResult(Unit unit) {
            Unit = unit;
        }
    }
}