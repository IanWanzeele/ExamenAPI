$(document).ready(function () {
    $('.star-button').each(function () {
        var button = $(this);
        var restaurantId = button.data('restaurant-id');
        $.get('/Restaurants/IsFavorite?restaurantId=' + restaurantId, function (isFavorite) {
            if (isFavorite) {
                button.addClass('favorite');
            } else {
                button.removeClass('favorite');
            }
        });
    });
});