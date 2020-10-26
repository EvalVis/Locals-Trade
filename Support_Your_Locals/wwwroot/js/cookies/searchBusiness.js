// Original JavaScript code by Chirp Internet: www.chirp.com.au
// Please acknowledge use of this code by including this header.

var today = new Date();
var expiry = new Date(today.getTime() + 1200 * 1000); // 20 minutes.

function setCookie(name, value)
{
    document.cookie = name + "=" + escape(value) + "; path=/; expires=" + expiry.toGMTString();
}

function storeValues(form) {
    setCookie("OwnersSurname", form.getElementById("OwnersSurname").value);
    setCookie("Header", form.getElementById("Header").value);
    setCookie("SearchInDescription", form.getElementById("SearchInDescription").value);
    //setCookie("WeekdaySelected", form.WeekdaySelected.value);
    return true;
}

function getCookie(name) {
    var re = new RegExp(name + "=([^;]+)");
    var value = re.exec(document.cookie);
    return (value != null) ? unescape(value[1]) : null;
}
