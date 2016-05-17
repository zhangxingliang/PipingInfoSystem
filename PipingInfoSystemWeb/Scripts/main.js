var app = angular.module("app", []);
app.service("utilsService", ['$http', '$q', function ($http, $q) {
    function getUrl(url, par) {
        if (par) {
            var q = '';
            if (url.indexOf('?') >= 0) {
                q = '&';
            } else {
                q = '?';
            }

            for (var k in par) {
                if (typeof par[k] === "undefined") {
                    continue;
                }
                if (q.length > 1) {
                    q += '&';
                }
                q = q + k + "=" + encodeURIComponent(par[k]);
            }

            url = url + q;
        }
        return url;
    }

    this.get = function (path, qs, success) {

        var url = getUrl(path, qs);
        return $http.get(url).then(function (r) {
            return $q.resolve(r.data);
        }, function (r) {
            if (r && r.status == 401) {
                return $q.reject({ code: '401', msg: '登录已失效，请刷新后重新登录' });
            } else {
                return $q.reject({ code: '500', msg: '网络错误，请检查您的网络连接' });
            }
        });

    };

    this.post = function (path, qs, data) {
        var url = getUrl(path, qs);
        return $http.post(url, data).then(function (r) {
            return $q.resolve(r.data);
        }, function (r) {
            if (r && r.status == 401) {
                return $q.reject({ code: '401', msg: '登录已失效，请刷新后重新登录' });
            } else {
                return $q.reject({ code: '500', msg: '网络错误，请检查您的网络连接' });
            }
        });

    };

}]);


app.service('mainService', ['$http', 'utilsService', function ($http, utilsService) {
    this.Search = function (request) {
       return utilsService.post("/api/main/search",{usertoken:"123"}, request);
    }

    this.Delete = function (id) {
        return utilsService.get("/api/main/delete",{usertoken:"123",pipingid:id});
    }

    this.GetInfo = function (id) {
        return utilsService.get("/api/main/getinfo", { usertoken: "123", pipingid: id });
    }
}]);

app.controller("mainCtrl", ['$scope', 'mainService', function ($scope, mainService) {
    $scope.datasource = [];
    $scope.pages = [];
    $scope.searchmodel= {
        startNo: '',
        year: ['',2010,2011,2012,2013,2014,2015,2016],
        diameter: [],
        type: ['','PCV','PRV','混凝土','橡胶'],
        texture:[]
    };
    $scope.searchrequst = {
        startNo: '',
        year: '',
        diameter:'' ,
        type: '',
        texture: ''
    };
    $scope.selectYear = '';
    $scope.flag = true;
    $scope.selectType = '';
    $scope.modifyoraddmodel = {};
    $scope.a ='123';
    $scope.Search = function () {
        var req = {
            startNo: $scope.searchrequst.startNo,
            year: $scope.selectYear,
            type: $scope.selectType
        }
        mainService.Search(req).then(function (r) {
            $scope.datasource = r.ext;
        });
    }

    $scope.Delete = function (row) {
        mainService.Delete(row.PipingID).then(function (r) {
            if (r.code == '0') {
                $scope.Search();
            }
        });
    }

    $scope.Modify = function (row) {
        mainService.GetInfo(row.PipingID).then(function (r) {
            if (r.code == '0') {
                $scope.modifyoraddmodel = r.ext;
                $scope.flag = false;
            }
        });
        
    }
}]);