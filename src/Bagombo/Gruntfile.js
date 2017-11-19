/// <binding BeforeBuild='clean:build, copy:build' Clean='clean:clean' />
/*
This file in the main entry point for defining grunt tasks and using grunt plugins.
Click here to learn more. https://go.microsoft.com/fwlink/?LinkID=513275&clcid=0x409
*/
module.exports = function (grunt) {
  grunt.initConfig({
    clean: {
      build: ['wwwroot/*'],
      clean: ['wwwroot/*']
    },
    copy: {
      build: {
        files: [
          //{ expand: true, src: ['bower_components/**/*.min.css'], dest: 'wwwroot/lib/css/', filter: 'isFile', flatten: true },
          //{ expand: true, src: ['bower_components/highlightjs/styles/default.css'], dest: 'wwwroot/lib/css/', rename: function (dest, src) { return dest + "highlight-default.css"; } },
          //{ expand: true, src: ['bower_components/**/*.min.js'], dest: 'wwwroot/lib/js/', filter: 'isFile', flatten: true },
          //{ expand: true, src: ['bower_components/bootstrap/fonts/*'], dest: 'wwwroot/lib/fonts/', filter: 'isFile', flatten: true },
          { expand: true, src: ['node_modules/highlightjs/styles/default.css'], dest: 'wwwroot/lib/css/', rename: function (dest, src) { return dest + "highlight-default.css"; } },
          { expand: true, src: ['node_modules/bootstrap/**/*.min.css'], dest: 'wwwroot/lib/css/', filter: 'isFile', flatten: true },
          { expand: true, src: ['node_modules/fontawesome/**/*.min.css'], dest: 'wwwroot/lib/css/', filter: 'isFile', flatten: true },
          { expand: true, src: ['node_modules/highlightjs/**/*.min.js'], dest: 'wwwroot/lib/js/', filter: 'isFile', flatten: true },
          { expand: true, src: ['node_modules/bootstrap/**/*.min.js'], dest: 'wwwroot/lib/js/', filter: 'isFile', flatten: true },
          { expand: true, src: ['node_modules/popper.js/**/*.min.js'], dest: 'wwwroot/lib/js/', filter: 'isFile', flatten: true },
          { expand: true, src: ['node_modules/jquery/dist/jquery.min.js'], dest: 'wwwroot/lib/js/', filter: 'isFile', flatten: true },
          { expand: true, src: ['css/*'], dest: 'wwwroot/lib/css/', filter: 'isFile', flatten: true },
          { expand: true, src: ['javascript/*.js'], dest: 'wwwroot/lib/js/', filter: 'isFile', flatten: true },
          { expand: true, src: ['assets/favicon.ico'], dest: 'wwwroot/', filter: 'isFile', flatten: true }
        ]
      }
    }
  });

  grunt.loadNpmTasks("grunt-contrib-clean");
  grunt.loadNpmTasks("grunt-contrib-copy");
};


