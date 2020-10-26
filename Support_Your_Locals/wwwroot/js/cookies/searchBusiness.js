// Original JavaScript code by Chirp Internet: www.chirp.com.au
// Please acknowledge use of this code by including this header.

var today = new Date();
var expiry = new Date(today.getTime() + 1200 * 1000); // 20 minutes.

window.onload = function() {
    loadCookies();
}

function setCookie(name, value)
{
    document.cookie = name + "=" + escape(value) + "; path=/; expires=" + expiry.toGMTString();
}

function storeValues() {
    setCookie("OwnersSurname", document.getElementById("OwnersSurname").value);
    setCookie("Header", document.getElementById("Header").value);
    setCookie("SearchInDescription", document.getElementById("SearchInDescription").value);
    //setCookie("WeekdaySelected", form.WeekdaySelected.value);
    return true;
}

function loadCookies() {
    document.getElementById("OwnersSurname").value = getCookie("OwnersSurname");
    document.getElementById("Header").value = getCookie("Header");
    document.getElementById("SearchInDescription").value = getCookie("SearchInDescription");
}

function getCookie(name) {
    var re = new RegExp(name + "=([^;]+)");
    var value = re.exec(document.cookie);
    return (value != null) ? unescape(value[1]) : null;
}
