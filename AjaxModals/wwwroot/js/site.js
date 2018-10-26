$(function () {
    var placeholderElement = $('#modal-placeholder');

    $(document).on('click', 'button[data-toggle="ajax-modal"]', function (event) {
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            placeholderElement.html(data);
            placeholderElement.find('.modal').modal('show');
        });
    });

    placeholderElement.on('click', '[data-save="modal"]', function (event) {
        event.preventDefault();

        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var dataToSend = new FormData(form.get(0));

        $.ajax({ url: actionUrl, method: 'post', data: dataToSend, processData: false, contentType: false }).done(function (data) {
            var newBody = $('.modal-body', data);
            placeholderElement.find('.modal-body').replaceWith(newBody);

            var isValid = newBody.find('[name="IsValid"]').val() === 'True';
            if (isValid) {
                var notificationsPlaceholder = $('#notification');
                var notificationsUrl = notificationsPlaceholder.data('url');
                $.get(notificationsUrl).done(function (notifications) {
                    notificationsPlaceholder.html(notifications);
                });

                var tableElement = $('#contacts');
                var tableUrl = tableElement.data('url');
                $.get(tableUrl).done(function (table) {
                    tableElement.replaceWith(table);
                });

                placeholderElement.find('.modal').modal('hide');
            }
        });
    });
});