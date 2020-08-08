

/* logo footer */

function logoFooter() {
    var hLogoSrc = $("header .header-logo img").attr("src");
    var fLogo = $("#logo-footer img");
    fLogo.attr("src", hLogoSrc);
}

$(document).ready(function () {
    logoFooter();
});