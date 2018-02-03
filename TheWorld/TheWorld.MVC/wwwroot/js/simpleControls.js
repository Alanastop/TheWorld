// simpleControls.js
(function () {
    "use strict";

    angular.module("simpleControls", [])
        .directive("waitCursor", waitCursor);

    function waitCursor() {
        return {
            scope: {
                show: "=displayWhen"
            },
            restrict: "E",
            templateUrl: "/views/waitCursor.html"
        };
    }

})();

//var simpleControls_library = {    
//    waitCursor: function () {
//        return { 
//            templateUrl: "/views/waitCursor.html/"
//        };
//    }
//}