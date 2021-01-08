/*/////////////////////////////////
            mobile-bar
/////////////////////////////////*/



$(document).ready(function(){
    
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
    $("#mobile-menu li.img-item a").click(function(){
        if($("#mobile-menu ul li.gayab").css("display")=="none"){
            $("#mobile-menu ul li.gayab").css("display","block");
        }
        else{
            $("#mobile-menu ul li.gayab").css("display","none");
        }
    });
    $("#mobile-nav-open-bar").click(function(){
        $("#mobile-menu ul li.gayab").css("display","none");
    });
    
});
