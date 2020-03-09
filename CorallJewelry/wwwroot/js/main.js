$('#multiCarousel').carousel({
    interval: 10000
})
$('.carousel .carousel-item').each(function(){
    var next = $(this).next();
    if (!next.length) {
        next = $(this).siblings(':first');
    }
    next.children(':first-child').clone().appendTo($(this));

    for (var i=0;i<2;i++) {
        next=next.next();
        if (!next.length) {
            next = $(this).siblings(':first');
        }

        next.children(':first-child').clone().appendTo($(this));
    }
});
function Strt() {
    setTimeout(function () { LoadOff() }, 200);
}
//$(window).on("load", function () {
//    setTimeout(function () { LoadOff() }, 2000);
//});

function RootingAnimation(millisecond) {
    LoadOn();
    setTimeout(function () { LoadOff() }, millisecond);
}
function LoadOn() {
    $("#right").css("transform", "translateX(0)");
    $("#left").css("transform", "translateX(0)");
    $(".item-door").css("box-shadow", "0 0px 0px 0px rgba(0,0,0,0), 0 0px 0px 0px rgba(0,0,0,0), 0 0px 0px 0 rgba(0,0,0,0)");
    setTimeout(function () { $("#preloader").removeAttr("hidden") }, 1000);

}
function LoadOff() {
    $("#preloader").attr("hidden", "");
    $("#right").css("transform", "translateX(-100%)");
    $("#left").css("transform", "translateX(100%)");
    $(".item-door").css("0 8px 10px 1px rgba(0,0,0,0.14), 0 3px 14px 3px rgba(0,0,0,0.12), 0 4px 5px 0 rgba(0,0,0,0.20)");
    setTimeout(function () {
        $(".wrapper-slider").css("z-index", "0");
    }, 500);
}/* JS Document */
Strt();
