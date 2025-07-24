$(document).ready(function () {
    $('#job-list-container').on('click', '.btn-toggle-save', function (e) {
        e.preventDefault();

        var button = $(this);
        var jobId = button.data('jobid');

        $.post('/Job/ToggleSave', { jobId: jobId }, function (response) {
            if (response.success) {
                var icon = button.find('i');
                if (response.saved) {
                    button.addClass('saved');
                    button.attr('title', 'Bỏ theo dõi');
                    icon.removeClass('fa-regular').addClass('fa-solid');
                } else {
                    button.removeClass('saved');
                    button.attr('title', 'Theo dõi');
                    icon.removeClass('fa-solid').addClass('fa-regular');
                }
            } else {
                if (response.redirectUrl) {
                    window.location.href = response.redirectUrl;
                } else {
                    alert(response.message || 'Có lỗi xảy ra.');
                }
            }
        });
    });
});