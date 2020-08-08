$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name] !== undefined) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};

var AjaxFilter = {
    url: false,
    query: '',
    baseurl: '',
    spinnerTemplate: "<div class=\"spinner products\"><div class=\"bounce1\"></div><div class=\"bounce2\"></div><div class=\"bounce3\"></div></div>",
    spinnerToggle: function (wantShow) {
        if (wantShow) {
            $(".ajax-products").html(this.spinnerTemplate);
            $(".ajax-products .spinner.products").show();
        } else {
            $(".ajax-products .spinner.products").hide();
        }
    },
    init: function (url, query, baseurl) {
        this.url = url;
        this.query = query.replace(/amp;/g, '');
        this.baseurl = baseurl;
    },
    setFilter: function (flt) {
        if (flt !== 'pagenumber' && flt !== 'update') {
            $("#PageNumber").val(0);
        }

        AjaxFilter.spinnerToggle(true);
        $.ajax({
            cache: false,
            url: this.url + this.query,
            data: { model: $('#ajaxfilter-form').serializeObject(), typ: flt },
            type: 'post',
            success: this.reloadFilters,
        });
    },
    reloadPages: function () {
        var pager = $('.pagination');
        if (void 0 != pager && pager.length > 0) {
            $("a", pager).each(function () {
                $(this).click(function (e) {
                    e.preventDefault();

                    var hrefAttr = $(this).attr("href");
                    hrefAttr = hrefAttr.split("pagenumber=")[1];

                    if (hrefAttr == undefined) {
                        hrefAttr = 1;
                    };
                    var pageNumber = hrefAttr - 1;
                    $("#PageNumber").val(pageNumber);
                    AjaxFilter.setFilter('pagenumber');
                });
            });
        }
        productInfo();
    },
    reloadFilters: function (response) {

        AjaxFilter.spinnerToggle(false);
        if (response.Data) {
            $('body,html').animate({ scrollTop: $('.ajax-products').position().top }, 'fast');
            if (response.Products != null) {
                $('.ajax-products').html(response.Products);
                AjaxFilter.reloadPages();
            }
            if (response.Url.length > 0) {
                history.pushState(null, null, response.Url);
                AjaxFilter.query = response.Url;
            }
            else {
                history.pushState(null, null, AjaxFilter.baseurl);
                AjaxFilter.query = '';
            }
            if (response.Data.AjaxProductsModel) {
                var count = response.Data.AjaxProductsModel.Count;
                $('.items-total').html(count);
                var pagenum = response.Data.AjaxProductsModel.PagingFilteringContext.PageNumber;
                var firstitem = response.Data.AjaxProductsModel.PagingFilteringContext.FirstItem;
                var lastitem = response.Data.AjaxProductsModel.PagingFilteringContext.LastItem;
                $('.number').html(firstitem + ' - ' + lastitem);
            }
            if (response.Type.length > 0) {
                if (response.Type != 'm') {

                    //$(".manufacturer-section input:checkbox").attr("disabled", true);
                    $(".manufacturer-section input:checkbox:not(:checked)").attr("disabled", true);
                    $('#manufacturers-filter-section .ajaxfilter-section ul li label span').each(function (entry) {
                        $(this).text('(0)');
                    });

                    $(".manufacturer-section select option").not("option:selected").not("option:first-of-type").attr('disabled', 'disabled');

                    if (response.Data.manufacturerModel.Manufacturers.length > 0) {
                        response.Data.manufacturerModel.Manufacturers.forEach(function (entry) {
                            $(".manufacturer-section").find("input[value=" + entry.Id + "]").attr("disabled", false);
                            $('#manufacturers-filter-section .ajaxfilter-section ul li[data-id=' + entry.Id + '] label span').text('(' + entry.Count + ')');
                            $(".manufacturer-section select option[value='" + entry.Id + "']").attr('disabled', false);
                        });
                    }

                }
                if (response.Type != 'v') {

                    $(".vendors-section input:checkbox").attr("disabled", true);
                    $('#vendors-filter-section .ajaxfilter-section ul li label span').each(function (entry) {
                        $(this).text('(0)');
                    });

                    $(".vendors-section select option").not("option:selected").not("option:first-of-type").attr('disabled', 'disabled');
                    if (response.Data.vendorsModel.Vendors.length > 0) {
                        response.Data.vendorsModel.Vendors.forEach(function (entry) {
                            $(".vendors-section").find("input[value=" + entry.Id + "]").attr("disabled", false);
                            $('#vendors-filter-section .ajaxfilter-section ul li[data-id=' + entry.Id + '] label span').text('(' + entry.Count + ')');
                            $(".vendors-section select option[value='" + entry.Id + "']").attr('disabled', false);
                        });
                    }

                }
                if (response.Type.startsWith('s')) {
                    var id = response.Type.split('-').pop();
                    var specSection = $('.specification-section .filter-section');
                    specSection.each(function (entry) {
                        var specification = $(this);

                        var idSpec = $(this).attr("data-id");
                        if (idSpec != id) {
                            $(this).find("input:checkbox").attr("disabled", true);
                            $(this).find("select option").not("option:selected").not("option:first-of-type").attr('disabled', 'disabled');

                            $('#specification-filter-section .specification-section .filter-section[data-id=' + idSpec + ']  .ajaxfilter-section ul li label span').each(function (entry) {
                                $(this).text('(0)');
                            });
                            if (response.Data.specyficationModel.SpecificationAttributes.length > 0) {
                                response.Data.specyficationModel.SpecificationAttributes.filter(function (item) {
                                    item.SpecificationAttributeOptions.filter(function (option) {
                                        $('#specification-filter-section .specification-section .filter-section[data-id=' + item.Id + '] .ajaxfilter-section ul li[data-id=' + option.Id + '] label span').text('(' + option.Count + ')');
                                    });
                                });
                            }

                        }
                        response.Data.specyficationModel.SpecificationAttributes.filter(function (item) {
                            if (item.Id == idSpec) {
                                item.SpecificationAttributeOptions.forEach(function (option) {
                                    specification.find("input[value=" + option.Id + "]").attr("disabled", false);
                                    specification.find("select option[value='" + option.Id + "']").attr('disabled', false);
                                });
                            }
                        });
                        specification.find('input[type=checkbox]:disabled:checked').attr('disabled', false);
                    });


                }
                else {
                    var specSection = $('.specification-section .filter-section');
                    specSection.each(function (entry) {
                        var specification = $(this);
                        $(this).find("input:checkbox").attr("disabled", true);
                        $(this).find("select option").not("option:selected").not("option:first-of-type").attr('disabled', 'disabled');
                        var idSpec = $(this).attr("data-id");
                        response.Data.specyficationModel.SpecificationAttributes.filter(function (item) {
                            if (item.Id == idSpec) {
                                item.SpecificationAttributeOptions.forEach(function (option) {
                                    specification.find("input[value=" + option.Id + "]").attr("disabled", false);
                                    specification.find("select option[value='" + option.Id + "']").attr('disabled', false);
                                });
                            }
                        });
                        specification.find('input[type=checkbox]:disabled:checked').attr('disabled', false);

                    });

                    $('#specification-filter-section .specification-section .filter-section .ajaxfilter-section ul li label span').each(function (entry) {
                        $(this).text('(0)');
                    });
                    if (response.Data.specyficationModel.SpecificationAttributes.length > 0) {
                        response.Data.specyficationModel.SpecificationAttributes.filter(function (item) {
                            item.SpecificationAttributeOptions.filter(function (option) {
                                $('#specification-filter-section .specification-section .filter-section[data-id=' + item.Id + '] .ajaxfilter-section ul li[data-id*=' + option.Id + '] label span').text('(' + option.Count + ')')
                            });
                        });
                    }
                }
                if (response.Type.startsWith('a')) {
                    var id = response.Type.split('-').pop();
                    var attrSection = $('#attribute-filter-section .filter-section').not("." + response.Type + "");
                    attrSection.each(function (entry) {
                        var attribute = $(this);
                        $(this).find("input:checkbox").attr("disabled", true);
                        $(this).find("select option").not("option:selected").not("option:first-of-type").attr('disabled', 'disabled');

                        var idattr = $(this).attr("data-id");
                        $('#attribute-filter-section .filter-section[data-id=' + idattr + '] .ajaxfilter-section ul li label span').each(function (entry) {
                            $(this).text('(0)');
                        });

                        response.Data.attributesModel.ProductVariantAttributes.filter(function (item) {
                            if (item.Id == idattr) {
                                item.ProductVariantAttributesOptions.forEach(function (option) {
                                    attribute.find("input[value='" + option.Name + "']").attr("disabled", false);
                                    attribute.find("select option[value='" + option.Name + "']").attr('disabled', false);
                                    $('#attribute-filter-section .filter-section[data-id=' + item.Id + '] .ajaxfilter-section ul li[data-id="' + option.Name + '"] label span').text('(' + option.Count + ')')
                                });
                            }
                            else {
                                item.ProductVariantAttributesOptions.forEach(function (option) {
                                    $('#attribute-filter-section .filter-section[data-id=' + item.Id + '] .ajaxfilter-section ul li[data-id="' + option.Name + '"] label span').text('(' + option.Count + ')')
                                });
                            }
                        });
                        attrSection.find('input[type=checkbox]:disabled:checked').attr('disabled', false);
                    });



                }
                else {
                    var attrSection = $('#attribute-filter-section .filter-section');
                    attrSection.each(function (entry) {
                        var attribute = $(this);
                        $(this).find("input:checkbox").attr("disabled", true);
                        $(this).find("select option").not("option:selected").not("option:first-of-type").attr('disabled', 'disabled');

                        var idattr = $(this).attr("data-id");
                        response.Data.attributesModel.ProductVariantAttributes.filter(function (item) {
                            if (item.Id == idattr) {
                                item.ProductVariantAttributesOptions.forEach(function (option) {
                                    attribute.find("input[value='" + option.Name + "']").attr("disabled", false);
                                    attribute.find("select option[value='" + option.Name + "']").attr('disabled', false);
                                });
                            }
                        });
                        attrSection.find('input[type=checkbox]:disabled:checked').attr('disabled', false);
                    });

                    $('#attribute-filter-section .filter-section .ajaxfilter-section ul li label span').each(function (entry) {
                        $(this).text('(0)');
                    });

                    if (response.Data.attributesModel.ProductVariantAttributes.length > 0) {
                        response.Data.attributesModel.ProductVariantAttributes.filter(function (item) {
                            item.ProductVariantAttributesOptions.filter(function (option) {
                                $('#attribute-filter-section .filter-section[data-id*=' + item.Id + '] .ajaxfilter-section ul li[data-id="' + option.Name + '"] label span').text('(' + option.Count + ')')
                            });
                        });
                    }

                }

            }
        }
    }
};

$.urlParam = function (purl, name) {
    var results = new RegExp('[\?&]' + name + '=([^&#]*)').exec(purl);
    if (results == null) {
        return null;
    }
    else {
        return decodeURI(results[1]) || 0;
    }
};

$(document).ready(function () {

    $.each($('#products-orderby option'), function (i, v) {
        var orderby = $.urlParam($(v).val().toLowerCase(), 'orderby');
        $(v).val(orderby);
    });
    $('#products-orderby').prop('onchange', '').unbind('onchange').change(function (e) {
        $("#SortOption").val(this.value);
        AjaxFilter.setFilter('orderby');
    });
    $("#SortOption").val($('#products-orderby').val());

    $.each($('#products-pagesize option'), function (i, v) {
        var pagesize = $.urlParam($(v).val().toLowerCase(), 'pagesize');
        $(v).val(pagesize);
    });

    if ($("#products-pagesize").length > 0) {
        $('#products-pagesize').prop('onchange', '').unbind('onchange').change(function (e) {
            $("#PageSize").val(this.value);
            AjaxFilter.setFilter('pagesize');
        });

        $("#PageSize").val($('#products-pagesize').val());
    }

    $(".viewmode-icon").on("click", function (e) {
        e.preventDefault();
        if ($(this).hasClass("grid")) {
            $('#ViewMode').val('grid');
            AjaxFilter.setFilter('viewmode');
        } else {
            $('#ViewMode').val('list');
            AjaxFilter.setFilter('viewmode');
        }
    });

    AjaxFilter.reloadPages();

});


function resetFiltersOnPrice() {

    if ($(".ajaxfilter-section input").is(':checked')) {
        $(".clearAllfilters").show();

    } else {

        $(".clearAllfilters").hide();

        (function getSelected() {
            var priceMin = $("#min-price").val(),
                priceCurrent = $("#price-current-min").val(),
                emptyVal = "",
                foo = [];

            $('.ajaxfilter-section select option:selected').each(function (i, selected) {
                foo[i] = $(selected).val();
            });

            foo = foo.filter(v => v != '');

            if (foo.length > 0 || priceMin !== priceCurrent) {
                $(".clearAllfilters").show();
            } else {
                $(".clearAllfilters").hide();
            }

        })();
    }
};


function resetFilters() {
    'use strict';
    var priceMin = $("#min-price").val(),
        priceCurrent = $("#price-current-min").val();

    $(".clearAllfilters").hide();

    if (priceMin !== priceCurrent) {
        $(".clearAllfilters").show();
    }

    if ($(".ajaxfilter-section input").is(':checked')) {
        $(".clearAllfilters").show();
    }

    (function getSelected() {
        var priceMin = $("#min-price").val(),
            priceCurrent = $("#price-current-min").val(),
            emptyVal = "",
            selectedValues = [];

        $('.ajaxfilter-section select option:selected').each(function (i, selected) {
            selectedValues[i] = $(selected).val();
        });


        selectedValues = selectedValues.filter(v => v != '');

        if (selectedValues.length > 0 || priceMin !== priceCurrent) {
            $(".clearAllfilters").show();
        }

    })();
};



function renderActiveFilters(renderClass, name) {

    var labelFor = $("." + renderClass).attr('id').replace("square_", "");

    function removeSingleFilter() {

        var getedClass = this.name.replace("filtredBy", "");
        $(".selectedOptions input[name = 'filtredBy" + renderClass + "' ].remover").unbind("click");
        $("input." + getedClass).click();

    }


    if ($(".square." + renderClass).hasClass("active")) {

        $('<div>').attr({
            class: 'col-12 px-0 itemHolder' + renderClass,
            readonly: true,
            name: "sector" + renderClass,
            value: name,
        }).appendTo('.selectedOptions');

        $('<input>').attr({
            class: 'btn btn-sm btn-secondary col-10',
            readonly: true,
            name: "filtredBy" + renderClass,
            value: name,
        }).appendTo('.itemHolder' + renderClass);

        $('<input>').attr({
            class: 'btn btn-sm btn-secondary remover col-2',
            readonly: true,
            name: "filtredBy" + renderClass,
            value: 'x',
        }).appendTo('.itemHolder' + renderClass);

        $(".selectedOptions input[name = 'filtredBy" + renderClass + "' ].remover").on('click', removeSingleFilter);

    } else {

        $('.itemHolder' + renderClass).remove();

    }

}

function listenToCheckBox(element, id, name) {
    $("." + id).toggleClass("active");
    renderActiveFilters(id, name);
};

//generate filter by section
function listenToSelect(selectId) {

    var newselectId = selectId;

    // reset filterby by select
    function removeSingleFilterSelect() {
        var getedClass = this.name.replace("filtredBy", "");

        $.when(
            $("select option").prop("disabled", false),
            $("select#" + selectId).find("option:first")
                .attr('selected', true)

            // hook reset filter
        ).then(

            AjaxFilter.setFilter(''),
            AjaxFilter.reloadPages(''),
            resetFilters(),
            $('.itemHolder' + selectId).remove()

        )
    };
    if ($('.itemHolder' + selectId).length > 0) {
        var value = $('.itemHolder' + selectId).remove();
    };

    if ($("select#" + selectId + " option:selected").val().length > 0) {

        var text = $("select#" + selectId + " option:selected").text();

        $('<div>').attr({
            class: 'col-12 px-0 itemHolder' + selectId,
            readonly: true,
            name: "sector" + selectId,
            value: name,
        }).appendTo('.selectedOptions');

        $('<input>').attr({
            class: 'btn btn-sm btn-secondary col-10',
            readonly: true,
            name: "filtredBy" + selectId,
            value: text,
        }).appendTo('.itemHolder' + selectId);

        $('<input>').attr({
            class: 'btn btn-sm btn-secondary remover col-2',
            readonly: true,
            name: "filtredBy" + selectId,
            value: 'x',
        }).appendTo('.itemHolder' + selectId);

        $(".selectedOptions input[name = 'filtredBy" + selectId + "' ].remover").on('click', removeSingleFilterSelect);

    } else {

        $('.itemHolder' + selectId).remove();

    }
};

$(function () {

    if (!$(".ajax-filter-section").is(':parent')) {
        $('.closeAllFilters').hide();
    };

    $(".closeAllFilters").on("click", function () {

        var that = $(this);

        if (that.hasClass("open")) {

            $(".filter-section .title").each(function () {

                $(".closeAllFilters").removeClass("open").addClass("close");

                $(this).removeClass("close").addClass("open").siblings(".ajaxfilter-section").slideUp("slow");
                $(this).find(".arrowHold").removeClass("rotate");

            })

        } else {

            $(".filter-section .title").each(function () {
                that.removeClass("close").addClass("open");
                $(".filter-section .title").removeClass("close").addClass("open");
                $(".ajaxfilter-section").slideDown("slow");
                $(".filter-section .title").find(".arrowHold").addClass("rotate");
            })
        };
    });

    // first load of window

    $.when(
        resetFilters()
    ).then(
        (function getSelectedAll() {

            var tab = [];

            $(".ajaxfilter-section input:checked").each(function () {

                var firstName = $(this).siblings("label").text().split("(")[0],
                    firstClass = $(this).attr("class");

                renderActiveFilters(firstClass, firstName);

            });

            $('.ajaxfilter-section select option:selected').each(function (i, selected) {

                if ($(selected).val().length > 0) {
                    var firstId = $(this).parents("select").attr("id");

                    listenToSelect(firstId)


                }

            });

        })()

    );


    $(".ajaxfilter-section select").on("click", function () {
        resetFilters();
    });

    // clear all click
    $(".clearAllfilters").on("click", function () {

        var priceMin = $("#min-price").val(),
            priceMax = $("#max-price").val(),
            priceCurrent = $("#price-current-min").val();

        // empty set filters box
        $(".selectedOptions").html("");

        // cumulate activity to default state
        $.when(

            $(".ajaxfilter-section input:checked").each(function () {
                $(this).prop("checked", false);
            }),

            $(".ajaxfilter-section input").each(function () {
                $(this).prop("disabled", false);
            }),

            $('.ajaxfilter-section select').each(
                function () {
                    $(this).find("option").removeAttr('selected').prop("disabled", false);
                    $(this).find("option:first").attr('selected', true);
                }
            ),

            $(".square.active").removeClass("active"),

            $("#slider-range").slider("values", 0, priceMin),
            $("#slider-range").slider("values", 1, priceMax)

            // hook reset filter
        ).then(

            AjaxFilter.setFilter(''),
            resetFilters(),
            $(this).hide()

        );
        // reset values in brackets
        $(".ajaxfilter-clear-price").click();

    });

    //open close title + arr rotate
    $(".filter-section .title").on("click", function () {
        $(this).siblings(".ajaxfilter-section").slideToggle("slow");
        $(this).find(".arrowHold").toggleClass("rotate");
    });

});

function productInfo() {
    $('.product-box').each(function () {
        var PB_bottom_h = $('.product-info .bottom', this).height();
        $('.box-unvisible', this).css('margin-bottom', - PB_bottom_h);
    });
}
