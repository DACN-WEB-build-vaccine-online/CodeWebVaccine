$(document).ready(function () {
    // Lấy ngày hôm nay
    var today = new Date();

    // Khởi tạo datepicker cho trường chọn ngày sinh
    $('.dateOfBirth').datepicker({
        dateFormat: 'yy-mm-dd',
        changeMonth: true,
        changeYear: true,
        yearRange: '1900:2023',
        beforeShowDay: function (date) {
            // Ngày không thể chọn (ngày sau ngày hôm nay) sẽ được làm xám
            if (date > today) {
                return [false, 'date-disabled', 'Ngày không thể chọn'];
            }
            return [true, ''];
        }
    });

    // Hàm validate trường
    function validateField(field) {
        const value = field.val().trim();
        const invalidFeedback = field.next('.invalid-feedback');

        if (value === '') {
            field.addClass('is-invalid');
            invalidFeedback.text('Vui lòng nhập trường này.');
        } else {
            field.removeClass('is-invalid');
            invalidFeedback.text('');
        }
    }

    // Xử lý khi form được submit
    $('#registrationForm').on('submit', function (event) {
        const fieldsToValidate = ['#fullName', '#dateOfBirth', '#province', '#district', '#ward', '#address'];

        for (const fieldId of fieldsToValidate) {
            validateField($(fieldId));
        }

        if (!this.checkValidity()) {
            event.preventDefault();
            event.stopPropagation();
        }

        this.classList.add('was-validated');
    });

    // Xử lý validate khi người dùng chuyển ra khỏi trường
    $('#fullName, #dateOfBirth, #province, #district, #ward, #address').on('blur', function () {
        validateField($(this));
    });

    // Xử lý validate khi người dùng thay đổi giá trị trường
    $('#fullName, #dateOfBirth, #province, #district, #ward, #address').on('change', function () {
        validateField($(this));
    });

    // Khóa trường quận huyện và phường xã ban đầu
    $('#district, #ward').prop('disabled', true);

    // Xử lý khi chọn thành phố
    $('#province').on('change', function () {
        const selectedCity = $(this).val();
        if (selectedCity) {
            // Cho phép chọn quận huyện khi đã chọn thành phố
            $('#district').prop('disabled', false);
        } else {
            // Khóa trường quận huyện khi không chọn thành phố
            $('#district').prop('disabled', true);
            // Khóa trường phường xã khi không chọn thành phố
            $('#ward').prop('disabled', true);
        }
    });

    // Xử lý khi chọn quận huyện
    $('#district').on('change', function () {
        const selectedDistrict = $(this).val();
        if (selectedDistrict) {
            // Cho phép chọn phường xã khi đã chọn quận huyện
            $('#ward').prop('disabled', false);
        } else {
            // Khóa trường phường xã khi không chọn quận huyện
            $('#ward').prop('disabled', true);
        }
    });
});
