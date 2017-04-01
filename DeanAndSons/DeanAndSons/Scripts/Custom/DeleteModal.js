//Fires once DOM is fully loaded but potentially before images and other items have fully loaded.
$(document).ready(function () {
    //Select cancel button of modal and assign onClick event to it
    $("#cancelDelete").click(function () {
        //Hide modal
        $("#deleteModal").modal("hide");
    });
})

//Dynamically attaches onclick event to all delete links.
$(document).on("click", ".deleteProp", function (event) {
    //console.log(event.target.id);
    //Takes controller and action URL and adds clicked link property ID to end ready for transmission to server delete method
    var selectedPropID = "/Propertys/DeleteLogical/" + event.target.id;
    //Changes action attr on modal submit form to correct property ID
    $("#deleteForm").attr("action", selectedPropID);

});