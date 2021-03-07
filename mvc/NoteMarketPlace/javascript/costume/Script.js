// JavaScript source code
/*////////////////////////////////////////////////
                        home
 /////////////////////////////////////////////////*/

$(document).ready(function () {

   
});

/*////////////////////////////////////////////////
                        mobile-bar
 /////////////////////////////////////////////////*/







$(document).ready(function(){
    showHideFunction();
    $(window).scroll(function(){
        showHideFunction();
    });
    
    function showHideFunction(){
        if ($(window).scrollTop() > 50) {
            $("nav").addClass("white-top");
            $(".navbar-brand img").attr("src","../images/FAQ/logo.png");
            $(".navbar-header span").addClass("change");
        } 
        else {
            $("nav").removeClass("white-top");
            $(".navbar-brand img").attr("src","../images/login/top-logo.png")
            $(".navbar-header span").removeClass("change");
        }
    };
    
    $("#mobile-nav-open-bar").click(function(){
        if( $(".w-navbar div").attr("class")== "navbar-header close-bar" ){
            $(".w-navbar div").removeClass("close-bar");
            $("#mobile-menu").css("width","0%");
        }
        else{
            $(".w-navbar div").addClass("close-bar");
            $("#mobile-menu").css("width","100%");
        }
    });
    
});































