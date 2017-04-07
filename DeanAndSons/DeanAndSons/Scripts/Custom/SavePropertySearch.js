////On DOM ready do following
//$(function () {
//    $("#savePS").on("click", saveSearch());

//});

//Dynamically attaches onclick event to all delete links.
$(document).on("click", "#savePS", function () {
    saveSearch();
});

function saveSearch() {

    //Make ajax call to method and call other methods once call complete
    $.ajax({
        type: 'get',
        contentType: "application/json",
        dataType: 'json',
        cache: false,
        url: '/Account/SaveSearch',
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
        "Location": $("#Location").val(),
        "Radius": $("#Radius").val(),
        "MinPrice": $("#MinPrice").val(),
        "MaxPrice": $("#MaxPrice").val(),
        "Beds": $("#Beds").val(),
        "Age": $("#Age").val(),
        "CategorySort": $("#CategorySort").val(),
        "OrderSort": $("#OrderSort").val()
    }
    //return result;
    return JSON.stringify(result);
}