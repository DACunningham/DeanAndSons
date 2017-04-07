//Dynamically attaches onclick saveProperty link.
$(document).on("click", "#saveProperty", function () {
    //Assign the correct action link URK to var
    var actionURL = '/Account/SaveProperty';
    //Call controller method via AJAX 
    save(actionURL);
});

//Dynamically attaches onclick event to all delete links.
$(document).on("click", "#savePropertyAnon", function () {
    //Assign the correct action link URK to var
    var actionURL = '/Account/SavePropertyAnon';
    //Call controller method via AJAX 
    save(actionURL);
});

function save(actionURL) {

    //Make ajax call to method and call other methods once call complete
    $.ajax({
        type: 'get',
        contentType: "application/json",
        dataType: 'json',
        cache: false,
        url: actionURL,
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

//Create JSON object with one key/value pair
function createJSON() {
    var result = {
        "PropertyID": $("#PropertyID").val()
    }
    //stringify the JSON object ready for transmission
    return JSON.stringify(result);
}