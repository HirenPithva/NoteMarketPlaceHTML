$(document).ready(function(){
    $(".toggelclas").click(function(){
        $(this).toggleClass("fa-eye fa-eye-slash");
        if($("#InputPassword1").attr("type") == "password"){
            $("#InputPassword1").attr("type","text");
        }
        else{
            $("#InputPassword1").attr("type","password");
        }
    });
});