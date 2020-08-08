//function prodAttrCookieRead() {
//    var prodId = $(".inside-form").data("5eb40f9d0f6c160fc46794a9")
//    console.log(Cookies.get(prodId));
//}


function Related() {
    var product = $(".product-details-page.product-scroll").length > 0;
    var related = $("#related_container");
    if (product) {
        $(related).removeClass("col-xxl-2 col-lg-3");
        var RelatedProducts = new Swiper('#RelatedProducts', {
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
                    slidesPerView: 2
                },
                1200: {
                    slidesPerView: 2
                },
                1400: {
                    slidesPerView: 3
                }
            }
        });
    } else {
        var RelatedProducts = new Swiper('#RelatedProducts', {
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
    }
}
function ScrollTab() {
    $('.custom-pills').each(function () {
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
    $(".tab-controls .prev").on("click", function () {
        var leftPos = $('.custom-pills').scrollLeft();
        $('.custom-pills').animate({ scrollLeft: leftPos - 200 }, 200);
    });
    $(".tab-controls .next").on("click", function () {
        var leftPos = $('.custom-pills').scrollLeft();
        $('.custom-pills').animate({ scrollLeft: leftPos + 200 }, 200);
    });
}

$(document).ready(function () {
    Related();
    ScrollTab();
    //prodAttrCookieRead();
});