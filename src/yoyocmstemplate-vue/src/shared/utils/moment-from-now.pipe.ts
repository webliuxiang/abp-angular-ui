import moment from 'moment';
import Vue from 'vue';


Vue.filter('momentFromNow', (value) => {
    if (!value) {
        return '';
    }

    return moment(value).fromNow();
});
