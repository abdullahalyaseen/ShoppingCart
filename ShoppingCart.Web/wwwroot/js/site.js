// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function getCookieValue(cname) { // cname is nothing but the cookie value which 
    //contains the value
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

function calculateRating() {
    let rateElements = document.getElementsByClassName("star-inner");
    for (let i = 0; i < rateElements.length; i++) {
        let rate = (rateElements[i].getAttributeNode("rating").value / 5 ) * 100;
        let stars = `${rate}%`;
        document.getElementById(rateElements[i].id).style.width = stars;
    }
}