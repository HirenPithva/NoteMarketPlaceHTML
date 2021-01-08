$(document).ready(function(){
    /*showHideFunction();
    $(window).scroll(function(){
        showHideFunction();
    });
    
    function showHideFunction(){
        if ($(window).scrollTop() > 50) {
            $("nav").addClass("white-top");
            $(".navbar-header span").addClass("change");
        } 
        else {
            $("nav").removeClass("white-top");
            $(".navbar-header span").removeClass("change");
        }
    };*/
    
    $("#mobile-nav-open-bar").click(function(){
        if( $(".w-navbar div").attr("class")== "navbar-header close-bar" ){
            $(".w-navbar div").removeClass("close-bar");
            $("#mobile-menu").css("width","0%");
            $("#mobile-menu ul li.gayab").css("display","none");
            $("#mobile-menu ul li.notes-gayab").css("display","none");
            $("#mobile-menu ul li.setting-gayab").css("display","none");
        }
        else{
            $(".w-navbar div").addClass("close-bar");
            $("#mobile-menu").css("width","100%");
        }
    });
    
    $("#mobile-menu li.img-item a").click(function(){
        if($("#mobile-menu ul li.gayab").css("display")=="none"){
            $("#mobile-menu ul li.gayab").css("display","block");
        }
        else{
            $("#mobile-menu ul li.gayab").css("display","none");
        }
    });
    $("#mobile-menu li.notes-item a").click(function(){
        if($("#mobile-menu ul li.notes-gayab").css("display")=="none"){
            $("#mobile-menu ul li.notes-gayab").css("display","block");
        }
        else{
            $("#mobile-menu ul li.notes-gayab").css("display","none");
        }
    });
    $("#mobile-menu li.setting-item a").click(function(){
        if($("#mobile-menu ul li.setting-gayab").css("display")=="none"){
            $("#mobile-menu ul li.setting-gayab").css("display","block");
        }
        else{
            $("#mobile-menu ul li.setting-gayab").css("display","none");
        }
    });
    
});


















