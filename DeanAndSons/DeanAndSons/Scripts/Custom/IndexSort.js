//Fires once DOM is fully loaded but potentially before images and other items have fully loaded.
$(document).ready(function () {
    //Set value of currently selected sorts persistently through pagination clicks
    var cat = catsel.charAt(0);
    var sort = catsel[1];
    $("#CategorySort").val(cat);
    $("#OrderSort").val(sort);
})