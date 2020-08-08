// menu scrolls up and down

function MenuScroll() {
    var body = document.body;
    var scrollUp = "scroll-up";
    var scrollDown = "scroll-down";
    var onTop = "onTop";
    let lastScroll = 0;
    var menuContainer = $(".mainNav");
    var menuBtn = $(".mainNav-button");

    if ($(".home-page").length > 0) {
        $('body').addClass('home');
    }

    $(document).scroll(function () {
        var currentScroll = window.pageYOffset;
        if ($(body).hasClass('home')) {
            scrollValue = 250;
        } else {
            scrollValue = 10;
        }

        if ($(this).scrollTop() <= scrollValue) {
            $(body).addClass(onTop);
            if (!$(menuBtn).hasClass("clicked")) {
                if (!$(menuBtn).hasClass("standard")) {
                    menuContainer.addClass("show");
                }
            }
        } else {
            $(body).removeClass(onTop);
            menuContainer.removeClass("show");
        }
        if (currentScroll > lastScroll && !$(body).hasClass(scrollDown)) {
            // down
            $(body).removeClass(scrollUp);
            if (lastScroll !== 0) {
                $(body).addClass(scrollDown);
                return;
            } else {
                $(body).removeClass(scrollDown);
            }
        } else if (currentScroll < lastScroll && $(body).hasClass(scrollDown)) {
            // up
            $(body).removeClass(scrollDown);
            $(body).addClass(scrollUp);
        }
        lastScroll = currentScroll;
    });
}

// main menu system

function mainMenuReplace() {

    var shoppingLinks = $(".shopping-links");
    var shoppingLinksContainer = $(".HeadBottom .shopping-links-container");
    var logo = $(".HeadTop .header-logo");
    var searchMobile = $(".mobile-search-trigger");
    var TopLinks = $(".HeaderLinks");
    var SelectorsD = $(".selectors-desktop");
    var Selectors = $(".selectors-container");
    var UserPanelH = $(".user-panel .user-panel-head");

    if (window.matchMedia('(max-width: 991px)').matches) {
        $('.mainNav .navbar-collapse').prependTo('#mobile_menu');
        Popper.Defaults.modifiers.computeStyle.enabled = false;

        $("#mobile_menu .nav-item.dropdown .dropdown-toggle").each(function () {
            $(this).on("click", function (e) {
                e.preventDefault();
                $(this).parent().addClass("show");
                $(this).parent().find(".dropdown-menu:first").addClass("show");
            });
        });
        $("#mobile_menu .nav-item.cat-back").each(function () {
            $(this).on("click", function () {
                $(this).parents(".dropdown:first").removeClass("show");
                $(this).parents(".dropdown-menu:first").removeClass("show");
            });
        });

        $(Selectors).insertAfter(UserPanelH);
        $(TopLinks).insertAfter(logo);
        $(searchMobile).insertAfter(logo);
    }
    else {
        $(TopLinks).prependTo(".hl-desktop")
        $(shoppingLinks).prependTo(shoppingLinksContainer);
        $(searchMobile).prependTo(".formSearch");
        $(Selectors).prependTo(SelectorsD);
        $('#mobile_menu .navbar-collapse').prependTo('.mainNav');
        Popper.Defaults.modifiers.computeStyle.enabled = true;
    }
}

/* menu only desktop */


function dropFix() {
    if (window.matchMedia("(min-width: 992px)").matches) {
        var dropStandard = $('.mainNav .navbar-nav > .dropdown.nav-item.standard > .dropdown-menu');
        if (dropStandard.length > 0) {
            var dropStandardPos = $('.mainNav .navbar-nav > .dropdown.nav-item.standard').position().top;
            $(dropStandard).attr('style', 'top:' + dropStandardPos + 'px');
        }
    }
}

function menuDesktop() {
    if (window.matchMedia("(min-width: 992px)").matches) {

        var dropdownWidth = $(".HeadBottom").width() - $(".mainNav").width() - $(".hl-desktop").width() - 50;

        $(".mainNav .dropdown-menu.columns").each(function () {
            $(this).attr("style", "width:" + dropdownWidth + "px");
        });

        $(".mainNav .navbar-nav > .dropdown").mouseenter(function () {
            $(this).addClass('show');
            $('.dropdown-menu:first', this).addClass('show');
            $(".backdrop-menu").addClass("show");
            //$('> .nav-link', this).attr('style', 'color:#fff;background:#292929;');
        });
        $(".mainNav .navbar-nav > .dropdown").mouseleave(function () {
            $(this).removeClass('show');
            $('.dropdown-menu:first', this).removeClass('show');
            $('> .nav-link', this).removeAttr('style');
            $(".backdrop-menu").removeClass("show");
        });

        var menuBtn = $(".mainNav-button");
        var menuContainer = $(".mainNav");

        var headerH = $("header").height();
        var menuContainerInside = $(".mainNav .navbar-nav");

        $(menuContainerInside).attr("style", "max-height:calc(100vh - " + headerH + "px - 45px)");

        menuBtn.on("click", function () {
            menuContainer.toggleClass("show");
            $(this).toggleClass("clicked");
            dropFix();
        });
    }
}


/* other links */

function soloLinks() {

    if (window.matchMedia("(min-width: 992px)").matches) {


        $("header .solo-link-item").each(function () {
            $(this).prependTo(".solo-link-container ul");
        });

    } else {

        $("header .solo-link-item").each(function () {
            $(this).prependTo("#mobile-menu .navbar-nav");
        });

    }
}

/* login check */

function loginCheck() {

    var logout = $(".user-panel-content .sidebar-info").length;
    var userlogin = $(".user-panel-trigger .title").attr("data-title-login");
    var userlogout = $(".user-panel-trigger .title").attr("data-title-logout");
    var title = $(".user-panel-trigger .title");

    if (logout > 0) {
        title.text(userlogout);
    } else {
        title.text(userlogin);
    }
}

$(document).ready(function () {

    mainMenuReplace();
    loginCheck();
    soloLinks();
    MenuScroll();
    menuDesktop();
    dropFix();

    $(window).resize(function () {
        mainMenuReplace();
        soloLinks();
    });

});