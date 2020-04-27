/// <binding BeforeBuild='default' Clean='clean_scripts' />
"use strict";

const gulp = require("gulp"),
      merge = require("merge-stream"),
      sass = require("gulp-sass"),
      rimraf = require("rimraf"),
      concat = require("gulp-concat"),
      cssmin = require("gulp-cssmin"),
      rename = require("gulp-rename"),
      uglify = require('gulp-uglify-es').default;

sass.compiler = require('node-sass');

var paths = {
    webroot: "./wwwroot/"
};

paths.baseJs = paths.webroot + "dist/js/";
paths.js = paths.baseJs + "**/*.js";
paths.minJs = paths.baseJs + "**/*.min.js";
paths.concatJsDest = paths.baseJs + "site.min.js";

paths.baseCss = paths.webroot + "dist/css/";
paths.css = paths.baseCss + "**/*.css";
paths.minCss = paths.baseCss + "**/*.min.css";
paths.concatCssDest = paths.baseCss + "site.min.css";

paths.baseLib = paths.webroot + "dist/lib/";
paths.libJs = paths.baseLib + "**/*.js"
paths.libCss = paths.baseLib + "**/*.css"

var packagesToImport = {
    "bulma": {
        "css/*": ""
    },
    "@fortawesome": {
        "fontawesome-free/css/all.*": "",
        "fontawesome-free/webfonts/*": "../webfonts"
    }
};


gulp.task("clean:lib", function (cb) {
    rimraf(paths.baseLib, cb);
});

gulp.task("clean:js", function (cb) {
    rimraf(paths.baseJs, cb);
});

gulp.task("clean:css", function (cb) {
    rimraf(paths.baseCss, cb);
});

gulp.task("clean", gulp.series("clean:js", "clean:css", "clean:lib"));

gulp.task("min:js", function () {
    return gulp.src([paths.js, "!" + paths.minJs], { base: "." })
        .pipe(uglify())
        .pipe(rename({ suffix: '.min' }))
        .pipe(gulp.dest('.'));
});

gulp.task("min:css", function () {
    return gulp.src([paths.css, "!" + paths.minCss])
        .pipe(concat(paths.concatCssDest))
        .pipe(cssmin())
        .pipe(gulp.dest("."));
});

gulp.task("min", gulp.series(["min:js", "min:css"]));

gulp.task("move-packages", function () {
    var streams = [];
    for (var prop in packagesToImport) {
        console.log("Prepping Scripts for: " + prop);
        for (var itemProp in packagesToImport[prop]) {
            streams.push(gulp.src("node_modules/" + prop + "/" + itemProp)
                .pipe(gulp.dest("wwwroot/dist/lib/" + prop + "/" + packagesToImport[prop][itemProp])));
        }
    }

    return merge(streams);

});

gulp.task('sass', function () {
    return gulp.src('content/scss/*.scss')
        .pipe(sass())
        .pipe(gulp.dest(paths.baseCss));
});

gulp.task('js', function () {
    return gulp.src('content/js/**/*.js')
        .pipe(gulp.dest(paths.baseJs));
});

// A 'default' task is required by Gulp v4
gulp.task("default", gulp.series(["clean", "sass", "js", "move-packages", "min"]));
