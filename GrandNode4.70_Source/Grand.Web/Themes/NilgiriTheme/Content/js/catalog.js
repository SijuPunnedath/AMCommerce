
function FeaturedProductsCatalog() {
    var FeaturedProducts = new Swiper('#FeaturedProducts', {
        speed: 400,
        spaceBetween: 30,
        navigation: {
            nextEl: '#FeaturedProducts .swiper-custom-next',
            prevEl: '#FeaturedProducts .swiper-custom-prev'
        },
        breakpoints: {
            0: {
                slidesPerView: 2,
                spaceBetween: 30
            },
            576: {
                slidesPerView: 3,
                spaceBetween: 30
            },
            1400: {
                slidesPerView: 4,
                spaceBetween: 30
            }
        }
    });
}


function MobileSibeBar() {
    var LeftSide = $(".generalLeftSide.SideBar");
    var RightSide = $(".generalSideRight");
    var SideBar = $(".SideBarMobile");
    var SideBarMobile = $(".SideBarMobile .generalLeftSide.SideBar");
    if (window.matchMedia('(max-width: 991px)').matches) {
        if (!$(SideBarMobile).length > 0) {
            $(LeftSide).prependTo(SideBar);
        }
    } else {
        if ($(SideBarMobile).length > 0) {
            $(SideBarMobile).insertBefore(RightSide);
        }
    }
}



function FullSideBar() {
    var LeftSideFull = $(".generalLeftSide.Full");
    var SideBarFull = $(".SideBarFull");

    $(LeftSideFull).prependTo(SideBarFull);
}

function sortContainers() {
    var so = $(".sort-options");
    var sp = $(".sort-size");
    var LeftSide = $(".generalLeftSide");
    var stats = $("#items_statistics");
    if (window.matchMedia('(max-width: 768px)').matches) {
        $(so).prependTo(LeftSide);
        $(sp).prependTo(LeftSide);
    } else {
        $(so).insertBefore(stats);
        $(sp).insertBefore(stats);
    }
}

// left-side canvas

function LeftSide() {
    if ($("#LeftSideCatalog").length > 0) {
        $('#LeftSideCatalog').insertAfter('main');
    }
    if ($("#LeftSideVendor").length > 0) {
        $('#LeftSideVendor').insertAfter('main');
    }
}

$(document).ready(function () {
    MobileSibeBar();
    FullSideBar();
    sortContainers();
    LeftSide();
    FeaturedProductsCatalog();

    $(window).resize(function () {
        MobileSibeBar();
        sortContainers();
    });
});