'use strict';

var app = angular.module('recordsApp', []);

app.controller('recordsCtrl', ['$scope', '$http',
    function ($scope, $http) {
        $scope.app = {};
        $scope.app.showAddNewRecord = false;

        $scope.app.loadData = function () {
            $http.get('/api/record').success(function (data) {
                $scope.app.records = data;
                loadCategories();
            }).error(function () {
                $scope.app.error = 'an unexpected error occurred';
            });
        };

        $scope.app.loadData();

        function loadCategories() {
            var runningTotal = [];
            $scope.app.categories = [];
            for (var i = 0; i < $scope.app.records.length; i++) {
                if (runningTotal.indexOf($scope.app.records[i].RecordCategory) < 0) {
                    runningTotal.push($scope.app.records[i].RecordCategory);
                    $scope.app.categories.push({ 'cat': $scope.app.records[i].RecordCategory });
                }
            }
        };

        $scope.app.clearSelectedCategory = function () {
            $scope.app.selectedCatForEdit = null;
        };

        $scope.app.findRecordForEditing = function (recordId) {
            for (var i = 0; i < $scope.app.records.length; i++) {
                if ($scope.app.records[i].RecordId == recordId) {
                    console.log('found, i: ' + i);
                    $scope.app.editRecordDate = $scope.app.records[i].RecordDate;
                    $scope.app.editRecordValue = $scope.app.records[i].RecordValue;
                    $scope.app.editRecordCategory = $scope.app.records[i].RecordCategory;
                    $scope.app.editedRecordId = recordId;
                    $scope.app.showAddNewRecord = false;
                    $scope.app.catFilter = "";
                    break;
                } //else {console.log('i: ' + i + ' at: ' + $scope.app.records[i].RecordDate + ' need: ' + dt); }              
            }
        }

        $scope.app.saveEdit = function () {
            if ($scope.app.editedRecordId != undefined) {
                var recordToEdit = {
                    RecordId: $scope.app.editedRecordId,
                    RecordDate: $scope.app.editRecordDate,
                    RecordValue: $scope.app.editRecordValue,
                    RecordCategory: $scope.app.editRecordCategory
                };
                var config = {
                    headers: { "CommandType": "UpdateRecord" }
                };
                $http.put('/api/record', recordToEdit, config)
                    .success(function (data, status, headers) {
                        console.log(headers('location'));
                        $scope.app.loadData();
                        $scope.app.clearEditingArea();
                    }).error(function () {
                        $scope.app.error = 'something\'s broken';
                    });
            }
        };

        $scope.app.clearEditingArea = function () {
            $scope.app.editedRecordId = undefined;
            $scope.app.editRecordDate = "";
            $scope.app.editRecordValue = "";
            $scope.app.editRecordCategory = "";
        }

        $scope.app.deleteRecord = function (id) {
            var config = {
                headers: { "CommandType": "DeleteRecord" }
            };
            $http.delete('/api/record/' + id, config).success(function (data, status, headers) {
                $scope.app.catFilter = "";
                $scope.app.loadData();
            });
        }

        $scope.app.displayAddNewRecordPanel = function () {
            console.log('here');
            $scope.app.showAddNewRecord = true;
        }
    }
]);

app.controller('newRecordCtrl', ['$scope', '$http',
    function ($scope, $http) {
        $scope.addApp = {};
        $scope.addNewRecord = function () {

            var config = {
                headers: { "CommandType": "AddRecord" }
            };
            var newRecord = {
                RecordDate: $scope.newRecordDate,
                RecordValue: $scope.newRecordValue,
                RecordCategory: $scope.newRecordCategory != undefined ?
                    $scope.newRecordCategory :
                    $scope.newRecordCategoryFromSelect.cat
            };
            $http.post('/api/record', newRecord, config).success(function (data, status, headers) {
                $scope.newRecordDate = "";
                $scope.newRecordValue = "";
                $scope.newRecordCategory = "";

                $http.get(headers('location')).success(function (data) {
                    $scope.app.loadData();
                    $scope.app.catFilter = "";
                });
            });

            $scope.app.showAddNewRecord = false;
        };

        $scope.cancelAddingNewRecord = function () {
            $scope.app.showAddNewRecord = false;
        };
    }]);

$(document).ready(function () {
    var newRecordDate = document.getElementById('newRecordDate');
    if (newRecordDate != null) {
        var dt = new Date();
        newRecordDate.value = (dt.getMonth() + 1) + '/' + dt.getDate() + '/' + dt.getFullYear();
    }
});