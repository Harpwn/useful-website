/// <binding BeforeBuild='default' Clean='clean_scripts' />
"use strict";

const gulp = require("gulp"),
    merge = require("merge-stream"),
    del = require("del");

gulp.task('clean_scripts', function () {
    return del(['wwwroot/dist/**', '!wwwroot/dist'], { force: true });
});

// Dependency Dirs
var packagesToImport = {
    "bulma": {
        "css/*": ""
    },
    "@fortawesome": {
        "fontawesome-free/css/all.*": "",
        "fontawesome-free/webfonts/*": "../webfonts"
    }
};

gulp.task("move_scripts", function () {

    var streams = [];

    for (var prop in packagesToImport) {
        console.log("Prepping Scripts for: " + prop);
        for (var itemProp in packagesToImport[prop]) {
            streams.push(gulp.src("node_modules/" + prop + "/" + itemProp)
                .pipe(gulp.dest("wwwroot/dist/" + prop + "/" + packagesToImport[prop][itemProp])));
        }
    }

    return merge(streams);

});

// A 'default' task is required by Gulp v4
gulp.task("default", gulp.series(["clean_scripts", "move_scripts"]));