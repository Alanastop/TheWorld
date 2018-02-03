//var the_world_library = {
//    myfunction : function() {
(function () {
    debugger;
    var ele = document.getElementById("username");
    ele.innerHTML = "Shawn Wildermuth";

    var main = document.getElementById("main");
    main.onmouseenter = function () {
        main.style.backgroundColor = "#888";
    }

    main.onmouseleave = function () {
        main.style.backgroundColor = "";
    }

    var menuItems = $("ul.menu li a");
    menuItems.on("click",
        function () {
            var me = $(this);
            alert(me.text());
        });
})();

//    }
//}