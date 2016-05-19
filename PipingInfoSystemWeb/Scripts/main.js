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
app.directive('fileModel', ['$parse', function ($parse) {
    return {
        restrict: 'A',
        link: function(scope, element, attrs, ngModel) {
            var model = $parse(attrs.fileModel);
            var modelSetter = model.assign;
            element.bind('change', function(event){
                scope.$apply(function(){
                    modelSetter(scope, element[0].files[0]);
                });
                //附件预览
                scope.file = (event.srcElement || event.target).files[0];
                scope.getFile();
            });
        }
    };
}]);
app.factory('fileReader', ["$q", "$log", function ($q, $log) {
    var onLoad = function (reader, deferred, scope) {
        return function () {
            scope.$apply(function () {
                deferred.resolve(reader.result);
            });
        };
    };

    var onError = function (reader, deferred, scope) {
        return function () {
            scope.$apply(function () {
                deferred.reject(reader.result);
            });
        };
    };

    var getReader = function (deferred, scope) {
        var reader = new FileReader();
        reader.onload = onLoad(reader, deferred, scope);
        reader.onerror = onError(reader, deferred, scope);
        return reader;
    };

    var readAsDataURL = function (file, scope) {
        var deferred = $q.defer();
        var reader = getReader(deferred, scope);
        reader.readAsDataURL(file);
        return deferred.promise;
    };

    return {
        readAsDataUrl: readAsDataURL
    };
}]);
app.controller("mainCtrl", ['$scope', 'mainService','fileReader', function ($scope, mainService, fileReader) {
    $scope.datasource = [];
    $scope.curdatasource = [];
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
    $scope.imageSrc = '';
    $scope.file = {};
    $scope.activePage = 1;
    $scope.pageList = [];
    $scope.Search = function () {
        var req = {
            startNo: $scope.searchrequst.startNo,
            year: $scope.selectYear,
            type: $scope.selectType
        }
        mainService.Search(req).then(function (r) {
            $scope.datasource = r.ext;
            $scope.pageList = [];
            $scope.activePage = 1;
            for (var i = 1; i <= parseInt((r.ext.length+14.9)/15); i++) {
                $scope.pageList.push(i);
            }
            $scope.selectPage(1);
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

    $scope.getFile = function () {
        fileReader.readAsDataUrl($scope.file, $scope)
                      .then(function (result) {
                          $scope.imageSrc = result;
                      });
    }

    $scope.Previous = function () {
        if ($scope.activePage > 1)
        {
            $scope.selectPage(--$scope.activePage);
        }
    }

    $scope.selectPage = function (page) {
        $scope.activePage = page;
        $scope.curdatasource = $scope.datasource.slice(15 * ($scope.activePage - 1), Math.min($scope.datasource.length, 15 * $scope.activePage));
    }
    $scope.Next = function () {
        if ($scope.activePage < $scope.pageList.length) {
            $scope.selectPage(++$scope.activePage);
        }
    }
}]);