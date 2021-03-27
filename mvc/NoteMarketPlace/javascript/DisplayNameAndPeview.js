$(function () {
    $(".multipleFile").change(function () {
        $(".MultiFileDiv p").text("");
        var i = 0;
        for (i; i < this.files.length; i++) {
            var fileName = this.files[i].name;
            if (i == 0) {
                $(".FirstPreviewName").text(fileName);
            }
            else {
                var text = $("<p></p>").text(fileName);
                $(".MultiFileDiv").append(text);
            }

        }
        if (i > 0) {
            //$(".NoteFilePreview").css("margin-top", "20px");
           
            //$(".MultiFileDiv p").css("font-size", "15px");
            filechangeCss();
        }
        else {
            $(".FirstPreviewName").text("Upload Your File");
            //$(".NoteFilePreview").css("margin-top", "");
           
            //$(".MultiFileDiv p").css("font-size", "");
            fileoriginalCss();
        }

    });
});
function filechangeCss() {
    $(".NoteFilePreview").css("margin-top", "20px");

    $(".MultiFileDiv p").css("font-size", "15px");
}
function fileoriginalCss() {
    $(".NoteFilePreview").css("margin-top", "");

    $(".MultiFileDiv p").css("font-size", "");
}
function changeCss(name, display) {
    $(display).css("margin-top", "20px");
    $(display).css("height", "50px");
    $(display).css("width", "40px");
    $(name).css("font-size", "15px");
    $(name).css("font-weight", "500");
    //$(name).css("color", "#333333");
}
function originalCss(name, display) {
    $(display).css("margin-top", "auto");
    $(display).css("height", "");
    $(display).css("width", "");
    $(name).css("font-size", "");
    $(name).css("font-weight", "");
    //$(name).css("color", "");
}
function ShowFilePreView(PreviewFile, DisplayPreviewFile, namePreviewFile) {
    if (PreviewFile.files && PreviewFile.files[0]) {
        var filename = $(PreviewFile)[0].files[0].name;
        $(namePreviewFile).text(filename);
        var mimeType = $(PreviewFile)[0].files[0]['type'];
        if (mimeType.split('/')[0] === 'image') {
            var Reader = new FileReader();
            Reader.onload = function (e) {
                $(DisplayPreviewFile).attr("src", e.target.result);
            }
            Reader.readAsDataURL(PreviewFile.files[0]);
            changeCss(namePreviewFile, DisplayPreviewFile);
        }


    }
    else {
        $(namePreviewFile).text("Upload Your Preview");
        $(DisplayPreviewFile).attr("src", "/images/User-Profile/upload.png");
        originalCss(namePreviewFile, DisplayPreviewFile);
    }
}
function ShowImagePreview(ImgFile, ImgPriview, ImgName) {

    if (ImgFile.files && ImgFile.files[0]) {
        var ImgFileName = $(ImgFile)[0].files[0].name;
        $(ImgName).text(ImgFileName);
        var reader = new FileReader();
        reader.onload = function (e) {
            $(ImgPriview).attr("src", e.target.result);
        }
        reader.readAsDataURL(ImgFile.files[0]);
        changeCss(ImgName, ImgPriview);

    }
    else {
        $(ImgName).text("Upload display Picture");
        $(ImgPriview).attr("src", "/images/User-Profile/upload.png");
        originalCss(ImgName, ImgPriview);

    }
}
