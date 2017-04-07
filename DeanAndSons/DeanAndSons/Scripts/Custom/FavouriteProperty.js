//Dynamically attaches onclick event to all delete links.
$(document).on("click", "#saveProperty", function () {
    save();
});

function save() {

    //Make ajax call to method and call other methods once call complete
    $.ajax({
        type: 'get',
        contentType: "application/json",
        dataType: 'json',
        cache: false,
        url: '/Account/SaveProperty',
        data: { obj: createJSON() },
        success: function (response, textStatus, jqXHR) {
            console.log(response.response);
            alert(response.response);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert('Error - ' + errorThrown);
        }
    });

}

function createJSON() {
    var result = {
        "PropertyID": $("#PropertyID").val()
    }
    //return result;
    return JSON.stringify(result);
}