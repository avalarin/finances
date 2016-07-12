/// <binding BeforeBuild='build' Clean='clean' />
'use strict';

var gulp = require('gulp');
var rimraf = require('rimraf');
var concat = require('gulp-concat');
var cssmin = require('gulp-cssmin');
var uglify = require('gulp-uglify');
var babel = require('gulp-babel');
var flatten = require('gulp-flatten');
var sass = require('gulp-sass');

function swallowError(error) {
    console.error(error.stack);
    this.emit('end');
}

gulp.task('clean', []);

gulp.task('build:scripts', function() {
    return gulp.src([ 'client/scripts/**/*.js' ])
        .pipe(babel({
            presets: ['react', 'es2015'],
            plugins: ['transform-es2015-modules-amd']
        }))
        .on('error', swallowError)
        .pipe(gulp.dest('wwwroot/scripts'));
});

gulp.task('build:styles:site', function() {
    return gulp.src(['client/styles/*.scss'])
        .pipe(sass())
        .pipe(gulp.dest('wwwroot/styles'));
});

gulp.task('build:styles:pages', function() {
    return gulp.src(['client/styles/pages/*.scss'])
        .pipe(sass())
        .pipe(gulp.dest('wwwroot/styles/pages'));
});

gulp.task('build:vendor', function () {
    gulp.src(['node_modules/font-awesome/fonts/*',
              'node_modules/open-sans-fontface/fonts/**/*'])
        .pipe(flatten())
        .pipe(gulp.dest('wwwroot/fonts/'));

    gulp.src(['node_modules/react/dist/react.js',
              'node_modules/react-dom/dist/react-dom.js',
              'node_modules/react-router/umd/ReactRouter.js',
              'node_modules/requirejs/require.js',
              'node_modules/marked/lib/marked.js',
              'node_modules/transliteration/transliteration.js',
              'node_modules/whatwg-fetch/fetch.js'])
        .pipe(flatten())
        .pipe(gulp.dest('wwwroot/vendor/'));
});

gulp.task('build:styles', ['build:styles:site', 'build:styles:pages']);
gulp.task('build', ['build:scripts', 'build:styles', 'build:vendor']);
