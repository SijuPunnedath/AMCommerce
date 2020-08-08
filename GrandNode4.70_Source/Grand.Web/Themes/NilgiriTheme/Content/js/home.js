$(function () {
    $('.custom-pills li:first-child a').tab('show');
});

function HomePills() {

    var recommededProducts = $(".home-page .recommended-product-grid");
    var customPills = $(".custom-pills");


    if ($(recommededProducts).length < 1) {
        $(customPills).removeClass("justify-content-lg-start").addClass("justify-content-center");
        $(customPills).parent().removeClass("col-xl-9 col-lg-8");
    } 

    $(".custom-pills li").each(function () {
        var indexof = $(this).index();
        if ($(".custom-tabs .tab-pane").eq(indexof).find("div").length < 1) {
            $(this).addClass("d-none");
        } else {
            $(this).removeClass("d-none");
        }
    });
}

var RecommendedProducts = new Swiper('#RecommendedProducts', {
    speed: 400,
    spaceBetween: 15,
    autoplay: {
        delay: 4000
    },
    breakpoints: {
        0: {
            slidesPerView: 1
        },
        576: {
            slidesPerView: 2
        },
        992: {
            slidesPerView: 1
        }
    }
});

var HomePageBlog = new Swiper('#HomePageBlog', {
    speed: 400,
    spaceBetween: 15,
    breakpoints: {
        0: {
            slidesPerView: 1
        },
        576: {
            slidesPerView: 2
        },
        1200: {
            slidesPerView: 3
        }
    }
});

var HomePageManufacturers = new Swiper('#HomePageManufacturers', {
    speed: 400,
    spaceBetween: 15,
    loop: true,
    autoplay: {
        delay: 3000
    },
    breakpoints: {
        0: {
            slidesPerView: 2
        },
        576: {
            slidesPerView: 3
        },
        1200: {
            slidesPerView: 4
        },
        1400: {
            slidesPerView: 5
        }
    }
});

var BestProd = new Swiper('#BestProd', {
    effect: 'coverflow',
    autoplay: {
        delay: 5000
    },
    coverflowEffect: {
        rotate: 50,
        slideShadows: true,
        stretch: -60
    },
    speed: 900,
    grabCursor: true,
    centeredSlides: true,
    slidesPerView: 2,
    loop: true,
    navigation: {
        nextEl: '.swiper-custom-next',
        prevEl: '.swiper-custom-prev'
    },
    pagination: {
        el: '.custom-pagination',
        type: 'bullets',
        clickable: true
    },
    breakpoints: {
        0: {
            slidesPerView: 1
        },
        576: {
            slidesPerView: 2
        }
    }
});

BestProd.on('transitionStart', function () {
    $("#BestProd .swiper-slide .product-box.landing .product-discount").removeClass('animated rubberBand');

});
BestProd.on('transitionEnd', function () {
    $("#BestProd .swiper-slide-active .product-box.landing .product-discount").addClass('animated rubberBand');
});

function homepageProductSlider() {

    if ($(".home-page .recommended-product-grid").length < 1) {
        slidesLg = 5;
    } else {
        slidesLg = 4;
    }

    $('.tab-pane.active.show .homepage-products-slider').each(function (index) {
        var SwiperProductFeatured = new Swiper($(this)[0], {
            pagination: {
                el: $(this).parent().find('.swiper-pagination'),
                clickable: true
            },
            navigation: {
                nextEl: $(this).find('.swiper-custom-next'),
                prevEl: $(this).find('.swiper-custom-prev')
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
                    slidesPerView: slidesLg,
                    spaceBetween: 30
                }
            }
        });
    });
}

function ScrollTab() {
    $('#pills-tab').each(function () {
        var TabW = $(this).width();
        var liItems = $(this);
        var Sum = 0;
        var TabControls = $(".tab-controls");
        if (liItems.children('li').length >= 1) {
            $(this).children('li').each(function (i, e) {
                Sum += $(e).outerWidth(true);
            });
            if (Sum >= TabW) {
                TabControls.addClass('show');
                $(this).addClass('show');
            } else {
                TabControls.removeClass('show');
                $(this).removeClass('show');
            }
        }
    });
}

$(".tab-controls .prev").on("click", function () {
    var leftPos = $('#pills-tab').scrollLeft();
    $('#pills-tab').animate({ scrollLeft: leftPos - 200},200);
});
$(".tab-controls .next").on("click", function () {
    var leftPos = $('#pills-tab').scrollLeft();
    $('#pills-tab').animate({scrollLeft: leftPos + 200},200);
});


$(document).ready(function () {

    HomePills();

    $('#pills-tab .nav-link').on('shown.bs.tab', function (e) {
        homepageProductSlider();
    });

    ScrollTab();

    $(window).resize(function () {
        ScrollTab();
    });
    $('.featured-product-slider').each(function (index) {
        var SwiperProductCategories = new Swiper($(this)[0], {
            slidesPerView: 2,
            slidesPerColumn: 2,
            spaceBetween: 0,
            pagination: {
                el: $(this).parent().find('.swiper-pagination'),
                clickable: true
            },
            navigation: {
                nextEl: $(this).find('.swiper-custom-next'),
                prevEl: $(this).find('.swiper-custom-prev')
            }
        });
    });
});
