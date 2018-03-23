if (window.location.hostname == 'localhost') {
    var urlAPI = 'http://' + window.location.hostname + '/LanHouse.API/api/';
    var urlAPIAuth = 'http://' + window.location.hostname + '/LanHouse.API/';

    var lan = '/EVIDENTESJ';
} else {
    var urlAPI = 'http://teste.api.lanhouse.bne.com.br/api/';
    var urlAPIAuth = 'http://teste.apiauth.lanhouse.bne.com.br/';
    var lan = window.location.pathname;
    var origem = window.location.host;
}

//var urlAPI = 'http://teste.api.lanhouse.bne.com.br/api/';
//var urlAPIAuth = 'http://teste.api.lanhouse.bne.com.br/';