class UtilsService {
    static getCookie(name) {
        let v = document.cookie.match('(^|;) ?' + name + '=([^;]*)(;|$)');
        return v ? v[2] : null;
    }

    // format price
    static formatThousands(n, dp) {
        let isNegative = (+n || 0) < 0;
        if (isNegative)
            n = (+n || 0) * -1;

        let s = '' + (Math.floor(n)),
            d = n % 1,
            i = s.length,
            r = '';

        while ((i -= 3) > 0) {
            r = ',' + s.substr(i, 3) + r;
        }

        let result = "";

        result += isNegative ? "-" : "";
        result += s.substr(0, i + 3);
        result += r;
        result += (d ? '.' + Math.round(d * Math.pow(10, dp || 2)) : '');

        return result;
    };

    static formatCurrency(input, blur) {
        // appends $ to value, validates decimal side
        // and puts cursor back in right position.

        // get input value
        var input_val = input.val();

        // don't validate empty input
        if (input_val === "") { return; }

        // original length
        var original_len = input_val.length;

        // initial caret position
        var caret_pos = input.prop("selectionStart");

        // check for decimal
        if (input_val.indexOf(".") >= 0) {

            // get position of first decimal
            // this prevents multiple decimals from
            // being entered
            var decimal_pos = input_val.indexOf(".");

            // split number by decimal point
            var left_side = input_val.substring(0, decimal_pos);
            var right_side = input_val.substring(decimal_pos);

            // add commas to left side of number
            left_side = this.formatNumber(left_side);

            // validate right side
            right_side = this.formatNumber(right_side);

            // On blur make sure 2 numbers after decimal
            if (blur === "blur") {
                right_side += "00";
            }

            // Limit decimal to only 2 digits
            right_side = right_side.substring(0, 2);

            // join number by .
            input_val = left_side;

        } else {
            // no decimal entered
            // add commas to number
            // remove all non-digits
            input_val = this.formatNumber(input_val);
            input_val = input_val;
        }

        // send updated string to input
        input.val(input_val);

        // put caret back in the right position
        var updated_len = input_val.length;
        caret_pos = updated_len - original_len + caret_pos;
        input[0].setSelectionRange(caret_pos, caret_pos);
    }
}