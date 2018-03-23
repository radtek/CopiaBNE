(function () {

    var app = angular.module('FacaCaveira', ['ngRoute']);

    app.config(function ($routeProvider, $locationProvider) {
        $routeProvider
         .when('/aluno', {
             templateUrl: 'views/aluno.html',
             controller: 'CadastroEscola',
             controllerAs: 'cadastro',
             resolve: {
                 // I will cause a 1 second delay
                 delay: function ($q, $timeout) {
                     var delay = $q.defer();
                     $timeout(delay.resolve, 1000);
                     return delay.promise;
                 }
             }
         })
        .when('/disciplina', {
            templateUrl: 'views/disciplina.html',
            controller: 'CadastroEscola',
            controllerAs: 'cadastro'
        })
        .when('/aluno-disciplina', {
            templateUrl: 'views/aluno_disciplina.html',
            controller: 'CadastroEscola',
            controllerAs: 'cadastro'
        })
        .otherwise({
            redirectTo: '/aluno'
        });
    });

    app.controller("CadastroEscola", ['$scope', '$http', function ($scope, $http, $location) {
        var store = this;


        store.alunos_cadastrados = [];

        store.disciplinas_cadastradas = [];


        $http.get("http://localhost:11293/api/aluno").success(function (data) {
            store.alunos_cadastrados = data;
            console.log('todos', data);
        });

        $http.get("http://localhost:11293/api/disciplina").success(function (data) {
            store.disciplinas_cadastradas = data;
        });


        store.aluno = {
            Nome: '',
            Id: 0
        }

        store.disciplina = {
            Id: 0,
            Nome: ''
        }

        store.busca = {
            Nome: ''
        }

        store.alunos_filtrados = []

        store.cadastro_aluno_concluido = false;

        store.cadastro_disciplina_concluido = false;

        $scope.CarregarAluno = function (id) {
            var aluno = $.grep(store.alunos_cadastrados, function (e) { return e.Id == id; });

            store.aluno.Id = aluno[0].Id;
            store.aluno.Nome = aluno[0].Nome;
        }

        $scope.carregarDisciplina = function (id) {
            var disciplina = $.grep(store.disciplinas_cadastradas, function (e) { return e.Id == id; })

            store.disciplina.Id = disciplina[0].Id;
            store.disciplina.Nome = disciplina[0].Nome;
        }

        $scope.cadastrarAluno = function () {
            console.log(store.aluno);
            $http.post("http://localhost:11293/api/aluno", store.aluno).success(function (data) {
                store.alunos_cadastrados.push(data);
                store.aluno = {};
            });
        }

        $scope.EditarAluno = function () {
            $http.put("http://localhost:11293/api/aluno/" + store.aluno.Id.toString(), store.aluno).success(function (data) {
                $.each(store.alunos_cadastrados, function (i, v) {
                    if (v.Id == data.Id) {
                        store.alunos_cadastrados.splice(i, 1);
                        store.alunos_cadastrados.push(data);
                    }
                })
                store.aluno = {};
            });
        }

        $scope.editarDisciplina = function () {
            $http.put("http://localhost:11293/api/disciplina/" + store.disciplina.Id.toString(), store.disciplina).success(function (data) {
                $.each(store.disciplinas_cadastradas, function (i, v) {
                    if (v.Id == data.Id) {
                        store.disciplinas_cadastradas.splice(i, 1);
                        store.disciplinas_cadastradas.push(data);
                    }
                })
                store.disciplina = {};
            })
        }

        $scope.buscarAlunos = function () {
            $http.get("http://localhost:11293/api/aluno?partialName=" + store.busca.Nome).success(function (data) {
                store.alunos_filtrados = data;
                console.log(data);
            });
            /*
            if(store.busca.Nome != ''){
                $http.get("http://localhost:11293/api/aluno?partialName=" + store.busca.Nome).success(function (data) {
                    store.alunos_filtrados = data;
                });
            } else {
                $http.get("http://localhost:11293/api/aluno").success(function (data) {



                    store.alunos_filtrados = data;
                });
            }*/
        }

        $scope.cadastrarDisciplina = function () {
            $http.post("http://localhost:11293/api/disciplina", store.disciplina).success(function (data) {
                store.disciplinas_cadastradas.push(data);
                store.disciplina = {};
            });
        }

        $scope.addAluno = function (idAluno, idDisciplina, $event) {
            var insert = $($event.target).is(':checked');

            if (insert) {
                $http.post("http://localhost:11293/api/disciplina/AddAluno/" + idDisciplina.toString() + '/' + idAluno.toString(), null).success(function (data) {
                });
            } else {
                $http.post("http://localhost:11293/api/disciplina/RemoveAluno/" + idDisciplina.toString() + '/' + idAluno.toString(), null).success(function (data) {
                });
            }

        }

        $scope.apagarAluno = function (id) {
            if (confirm("Deseja deletar?") == true) {
                $http.delete("http://localhost:11293/api/aluno/" + id.toString()).success(function (data) {
                    $.each(store.alunos_cadastrados, function (i, v) {
                        console.log(v);
                        if (v!= undefined && v.Id == id) {
                            store.alunos_cadastrados.splice(i, 1);
                        }
                    });
                });
            }
        }

        $scope.apagarDisciplina = function (id) {
            if (confirm("Deseja deletar?") == true) {
                $http.delete("http://localhost:11293/api/disciplina/" + id.toString()).success(function (data) {
                    $.each(store.disciplinas_cadastradas, function (i, v) {
                        if (v.Id == id) {
                            store.disciplinas_cadastradas.splice(i, 1);
                        }
                    });
                });
            }
        };

        //$scope.$watch('')

    }]);

    app.directive('filtroAluno', function () {
        return {
            restrict: 'E',
            require: 'ngModel',
            link: function (scope, elem, attr, ctrl) {

            }
        }
    })


    function outro_retorno(data) {
        console.log(data);
    }

    function addAlunoJs(idAluno, idDisciplina, check) {
        console.log(idAluno, idDisciplina, check);
    }



    //
})();