class StringFormat {
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
        let seconds = datetime.getSeconds();

        if (format == 'dd/MM/yyyy') {
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

            return  strDate + '/' + strMonth + '/' + strYear;
        }
        else if (format == 'MM/dd/yyyy') {
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
        else if (format == 'MM/dd/yyyy HH:mm') {
            let strDate = ''
            let strMonth = ''
            let strYear = year.toString();
            let strHour = ''
            let strMinute = ''
            let strSeconds = '';

            if (date < 10)
                strDate = '0' + date.toString();
            else
                strDate = date.toString();

            if (month < 10)
                strMonth = '0' + month.toString();
            else
                strMonth = month.toString();

            if (hour < 10)
                strHour = '0' + hour.toString();
            else
                strHour = hour.toString();

            if (minute < 10)
                strMinute = '0' + minute.toString();
            else
                strMinute = minute.toString();

            return strMonth + '/' + strDate + '/' + strYear + ' ' + strHour + ':' + strMinute;
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
        else if (format == 'dd/MM/yyyy HH:mm') {
            let strDate = ''
            let strMonth = ''
            let strYear = year.toString();
            let strHour = ''
            let strMinute = '';

            if (date < 10)
                strDate = '0' + date.toString();
            else
                strDate = date.toString();

            if (month < 10)
                strMonth = '0' + month.toString();
            else
                strMonth = month.toString();

            if (hour < 10)
                strHour = '0' + hour.toString();
            else
                strHour = hour.toString();

            if (minute < 10)
                strMinute = '0' + minute.toString();
            else
                strMinute = minute.toString();

            return strDate + '/' + strMonth + '/' + strYear + ' ' + strHour + ':' + strMinute;
        }
        else if (format == 'dd/MM/yyyy HH:mm:ss') {
            let strDate = ''
            let strMonth = ''
            let strYear = year.toString();
            let strHour = ''
            let strMinute = ''
            let strSeconds = '';

            if (date < 10)
                strDate = '0' + date.toString();
            else
                strDate = date.toString();

            if (month < 10)
                strMonth = '0' + month.toString();
            else
                strMonth = month.toString();

            if (hour < 10)
                strHour = '0' + hour.toString();
            else
                strHour = hour.toString();

            if (minute < 10)
                strMinute = '0' + minute.toString();
            else
                strMinute = minute.toString();

            if (seconds < 10)
                strSeconds = '0' + seconds.toString();
            else
                strSeconds = seconds.toString();

            return strDate + '/' + strMonth + '/' + strYear + ' ' + strHour + ':' + strMinute + ':' + strSeconds;
        } else if (format == 'dd/MM/yyyy hh:mm:ss tt') {
            let strDate = ''
            let strMonth = ''
            let strYear = year.toString();
            let strHour = ''
            let strMinute = ''
            let strSeconds = '';
            let strTT = 'AM';

            if (date < 10)
                strDate = '0' + date.toString();
            else
                strDate = date.toString();

            if (month < 10)
                strMonth = '0' + month.toString();
            else
                strMonth = month.toString();

            if (hour < 10)
                strHour = '0' + hour.toString();
            else if (hour < 12)
                strHour = hour.toString();
            else if (hour == 12) {
                strHour = hour.toString();
                strTT = 'PM';
            }
            else {
                strHour = (hour - 12).toString();
                strTT = 'PM';
            }

            if (minute < 10)
                strMinute = '0' + minute.toString();
            else
                strMinute = minute.toString();

            if (seconds < 10)
                strSeconds = '0' + seconds.toString();
            else
                strSeconds = seconds.toString();

            return strDate + '/' + strMonth + '/' + strYear + ' ' + strHour + ':' + strMinute + ':' + strSeconds + ' ' + strTT;
        }

        return datetime.toString();
    }
}