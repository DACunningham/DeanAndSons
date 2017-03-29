$(function () {
    //$("#submit").on("mousedown", function (e) {
    //    //console.log(tinyMCE.activeEditor.getContent());

    //    //Gets text from active editor
    //    var content = tinyMCE.activeEditor.getContent();
    //    $("#hTitle").val($("#titleDisp").html());
    //})

    $(".edit").on("blur", function (e) {
        console.log(e);
        //Gets text from active editor
        var content = tinyMCE.activeEditor.getContent();

        if (e.target.id === "descriptionDisp") {
            $("#hDescription").val(content);
        }
        else if (e.target.id === "titleDisp") {
            $("#hTitle").val(content);
        }
    })
});