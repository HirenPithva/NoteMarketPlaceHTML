$(document).ready(function(){
    showHideFunction();
    $(window).scroll(function(){
        showHideFunction();
    });
    
    function showHideFunction(){
        if ($(".navbar").scrollTop > 50) {
            $(".navbar").addClass("white-top");
        } else {
            $(".navbar").removeClass("white-top");
        }
    } 
});