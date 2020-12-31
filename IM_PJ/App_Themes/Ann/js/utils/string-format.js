﻿class StringFormat {
    contructor() {
        if (!!StringFormat._instance)
            return StringFormat._instance;

        StringFormat._instance = this;

        return this;
    }

    toTitleCase(str) {
        return str.replace(
          /\w\S*/g,
          function (txt) {
              return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase();
          }
        );
    }

    datetimeToString(strDate, format) {
        let datetime = new Date(strDate);
        let year = datetime.getFullYear();
        let month = datetime.getMonth() + 1;
        let date = datetime.getDate();
        let hour = datetime.getHours();
        let minute = datetime.getMinutes();

        if (format = 'MM/dd/yyyy') {
            let strDate = ''
            let strMonth = ''
            let strYear = year.toString();

            if (date < 10)
                strDate = '0' + date.toString();
            else
                strDate = date.toString();

            if (month < 10)
                strMonth = '0' + month.toString();
            else
                strMonth = month.toString();

            return strMonth + '/' + strDate + '/' + strYear;
        }
        else if (format == 'dd/MM') {
            let strDate = ''
            let strMonth = ''

            if (date < 10)
                strDate = '0' + date.toString();
            else
                strDate = date.toString();

            if (month < 10)
                strMonth = '0' + month.toString();
            else
                strMonth = month.toString();

            return strDate + '/' + strMonth
        }
        else if (format == 'HH:mm') {
            let strHour = ''
            let strMinute = ''

            if (hour < 10)
                strHour = '0' + hour.toString();
            else
                strHour = hour.toString();

            if (minute < 10)
                strMinute = '0' + minute.toString();
            else
                strMinute = minute.toString();

            return strHour + ':' + strMinute
        }

        return datetime.toString();
    }
}