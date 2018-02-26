(function () {
    "use strict";

    angular
        .module("app.layout", [])
        .controller("LayoutCtrl", layoutCtrl)
        .directive("handleEnter", function () {
            return function (scope, element, attrs) {
                element.bind("keydown keypress", function (event) {
                    if (event.which === 13) {
                        scope.$apply(function () {
                            scope.$eval(attrs.handleEnter);
                        });
                        event.preventDefault();
                    }
                });
            };
        });

    layoutCtrl.$inject = ['$scope', '$rootScope', '$http', '$cookie', '$modalWindowService', '$login', 'signalRSvc', '$anchorScroll', '$translate', '$map', 'urlSvc', '$location', '$validator'];
    function layoutCtrl($scope, $rootScope, $http, $cookie, $modalWindowService, $login, signalRSvc, $anchorScroll, $translate, $map, urlSvc, $location, $validator) {
        var vm = this;
        var sc = $scope;

        // application start -> user fullfill----------------------
        var tempPhoneCountryCode = "+00";

        $rootScope.user = {
            isAuthanticated: false,
            geocode: {
                location: {
                    place: {},
                    lat: 0,
                    lng: 0
                },
                phoneCountryCode: tempPhoneCountryCode
            }
        };

        $rootScope.setUpUser = function (user) {
            if (angular.equals({}, $rootScope.user.geocode.location.place)) {
                if (!angular.equals({}, user.geocode.location.place)) $rootScope.user.geocode.location = user.geocode.location;
            }
            if (angular.equals(tempPhoneCountryCode, $rootScope.user.geocode.phoneCountryCode)) {
                if (!angular.equals(tempPhoneCountryCode, user.geocode.phoneCountryCode) && !(typeof user.geocode.phoneCountryCode === "object"))
                    $rootScope.user.geocode.phoneCountryCode = user.geocode.phoneCountryCode;
            }
        }

        $cookie.getCookie();

        if (angular.equals(tempPhoneCountryCode, $rootScope.user.geocode.phoneCountryCode)) {
            $map.getPhoneCodeByLocation().then(function (code) {
                $rootScope.user.geocode.phoneCountryCode = code;
                sc.phoneCode = code;
            });
        } else {
            sc.phoneCode = $rootScope.user.geocode.phoneCountryCode;
        }

        var setLayout = function (data) {
            $rootScope.ParlourPath = data.ParlourPath;
            $rootScope.ImagePath = data.ImagePath;
            $rootScope.SessionId = data.SessionId;
            $rootScope.Groups = data.Groups;
            $rootScope.UserName = data.UserName;
            signalRSvc.initialize(sc.SessionId, sc.Groups);
        }

        $http.post(urlSvc.GetState).then(function (resp) {
            if (resp.data.IsEnabled && resp.data.IsAuthanticated) {
                $rootScope.user.isAuthanticated = true;
                setLayout(resp.data);
            }
        });
        // -------------------------------------------------------

        var templUrl = "/loginWizardContent.html";
        var url = "/Wizard/LoginWizard";
        var id = "loginWizard";

        var controller = layoutCtrl;

        vm.asideToggled = false;

        vm.openLoginModal = function () {
            $modalWindowService.openModal(url, id, templUrl, controller);
        };

        sc.OpenRialto = function () {
            if ($rootScope.user.isAuthanticated) {
                window.location = "/Rialto/Index";
            } else {
                vm.openLoginModal();
                $rootScope.goToRialto = true;
                return;
            }
        };

        sc.getClass = function (path) {
            if ($location.path() === path) return ($location.path()) ? "active" : "";
            return "";
        }

        sc.updateClass = function () {
            if ($location.path() === "/") {
                return ($location.path()) ? "index darktheme" : "";
            }
            if ($location.path() === "/Rialto/Index") {
                return ($location.path()) ? "darktheme" : "";
            }
            return "";
        }

        sc.$on("changeImgLayout", function (event, imagePath) {
            $rootScope.ImagePath = imagePath;
        });


        vm.logoff = function () {
            $http.get("/Account/LogOff").then(function () {
                window.location = "/";
            });
        };

        vm.gotoUp = function () {
            $anchorScroll();
        };

        sc.autoFocus = function (n, text) {
            if (text.length === 0) {
                if (n - 1 > 0) $("#smsCode" + (n - 1)).focus();
            }
            if (text.length === 2) {
                if (n + 1 < 5) $("#smsCode" + (n + 1)).focus();
                if (n + 1 === 5) $("#userName").focus();
            }
        }

        sc.setUpperCase = function (text) {
            if (text === undefined || text === null || text.toString() === NaN.toString()) return;
            sc.userName = text.toLowerCase().replace(/\b[a-z]/g, function (letter) {
                return letter.toUpperCase();
            });
        }

        var cleanStringFromIllegal = function (text) {
            if (text === undefined || text === null || text.toString() === NaN.toString()) return "";
            return text.replace(/\s+/, "").replace(".", "").replace(",", "").replace("-", "").replace("_", "");
        }

        // Handle Enter

        // Wizard navigator
        sc.wizard = {
            steps: {
                phoneStep: {
                    errors: {
                        hide: function () {
                            sc.formErrorPhone = false;
                            sc.formerrorValidation = false;
                            sc.formerrorServerError = false;
                            sc.formerrorSmsLimit = false;
                        },
                        showValidationError: function () {
                            sc.wizard.steps.phoneStep.errors.hide();
                            sc.formErrorPhone = sc.formerrorValidation = true;
                        },
                        showServerError: function () {
                            sc.wizard.steps.phoneStep.errors.hide();
                            sc.formErrorPhone = sc.formerrorServerError = true;
                        },
                        showSmsLimitError: function () {
                            sc.wizard.steps.phoneStep.errors.hide();
                            sc.formErrorPhone = sc.formerrorSmsLimit = true;
                        }
                    },
                    busy : false
                },
                passwordStep: {
                    errors: {
                        hide: function () {
                            sc.formErrorPassword = false;
                            sc.formerrorIncorrectPassword = false;
                        },
                        showIncorrectPasswordError: function () {
                            sc.wizard.steps.phoneStep.errors.hide();
                            sc.formErrorPassword = sc.formerrorIncorrectPassword = true;
                        }
                    }
                },
                codeStep: {
                    errors: {
                        hide: function () {
                            sc.formErrorCode = false;
                            sc.formerrorDoubleError = false;
                            sc.formerrorCodeNotMatch = false;
                            sc.formerrorNameValidation = false;
                            sc.formerrorCodeValidation = false;
                        },
                        showDoubleError: function () {
                            sc.wizard.steps.codeStep.errors.hide();
                            sc.formErrorCode = sc.formerrorDoubleError = true;
                        },
                        showCodeValidationError: function () {
                            sc.wizard.steps.codeStep.errors.hide();
                            sc.formErrorCode = sc.formerrorCodeValidation = true;
                        },
                        showNameValidationError: function () {
                            sc.wizard.steps.codeStep.errors.hide();
                            sc.formErrorCode = sc.formerrorNameValidation = true;
                        },
                        showMatchingError: function () {
                            sc.wizard.steps.codeStep.errors.hide();
                            sc.formErrorCode = sc.formerrorCodeNotMatch = true;
                        }
                    }
                }
            },
            position: {
                currentPosition: {
                    phoneStep: true,
                    codeStep: false,
                    passwordStep: false
                },
                resetPosition: function () {
                    sc.wizard.position.currentPosition.phoneStep = false;
                    sc.wizard.position.currentPosition.codeStep = false;
                    sc.wizard.position.currentPosition.passwordStep = false;
                },
                setPosition: {
                    gotoPhoneStep: function () {
                        sc.wizard.position.resetPosition();
                        sc.wizard.position.currentPosition.phoneStep = true;
                    },
                    gotoCodeStep: function () {
                        sc.wizard.position.resetPosition();
                        sc.wizard.position.currentPosition.codeStep = true;
                    },
                    gotoPasswordStep: function () {
                        sc.wizard.position.resetPosition();
                        sc.wizard.position.currentPosition.passwordStep = true;
                    }
                }
            },
            action: {
                cancel: function () {
                    $modalWindowService.closeModal(templUrl, controller);
                },
                stepUp: function () {
                    // check phone
                    if (sc.wizard.position.currentPosition.phoneStep) {
                        if (sc.wizard.steps.phoneStep.busy) return;
                        sc.wizard.steps.phoneStep.busy = true;
                        var phoneCode = cleanStringFromIllegal(sc.phoneCode);
                        var phoneNumber = cleanStringFromIllegal(sc.phoneNumber);
                        sc.wizard.steps.phoneStep.phone = phoneCode + phoneNumber;
                        if ($validator.phoneCode(phoneCode) && $validator.phoneNumber(phoneNumber)) {
                            sc.wizard.steps.phoneStep.errors.hide();
                            $login.checkPhone(sc.wizard.steps.phoneStep.phone, function (data) {
                                if (data.IsExist === false && data.SmsSended === false && data.LimitExceeded === false) {
                                    sc.wizard.steps.phoneStep.errors.showServerError();
                                }
                                if (data.IsExist === false && data.SmsSended === false && data.LimitExceeded === true) {
                                    sc.wizard.steps.phoneStep.errors.showSmsLimitError();
                                }
                                if (data.IsExist && !data.SmsSended) {
                                    sc.wizard.position.setPosition.gotoPasswordStep();
                                }
                                else if (!data.IsExist && data.SmsSended) {
                                    sc.wizard.position.setPosition.gotoCodeStep();
                                }
                                sc.wizard.steps.phoneStep.busy = false;
                            });
                        }
                        else {
                            sc.wizard.steps.phoneStep.errors.showValidationError();
                            sc.wizard.steps.phoneStep.busy = false;
                        }
                    }
                    // check password
                    else if (sc.wizard.position.currentPosition.passwordStep) {
                        $login.checkPass(sc.wizard.steps.phoneStep.phone, sc.password, function (data) {
                            if (data.IsEnabled) {
                                $rootScope.user.isAuthanticated = true;
                                setLayout(data);
                                if ($rootScope.goToRialto) window.location = "/Rialto/Index";
                                sc.wizard.action.cancel();
                            }
                            else {
                                sc.wizard.steps.passwordStep.errors.showIncorrectPasswordError();
                            }
                        });
                    }
                    // check code
                    else if (sc.wizard.position.currentPosition.codeStep) {
                        var code = cleanStringFromIllegal(sc.smsCode1 + sc.smsCode2 + sc.smsCode3 + sc.smsCode4);
                        var userName = cleanStringFromIllegal(sc.userName);
                        var codeValid = $validator.smsCode(code);
                        var nameValid = $validator.name(userName);
                        if (codeValid && nameValid) {
                            // registration
                            $login.registration(code, userName, function (data) {
                                if (data.IsEnabled || data.Success) {
                                    $rootScope.user.isAuthanticated = true;
                                    setLayout(data);
                                    if ($rootScope.goToRialto) window.location = "/Rialto/Index";
                                    sc.wizard.action.cancel();
                                }
                                else {
                                    sc.wizard.steps.codeStep.errors.showMatchingError();
                                }
                            });
                        }
                        else {
                            if (!codeValid && !nameValid) {
                                sc.wizard.steps.codeStep.errors.showDoubleError();
                            } else {
                                if (!codeValid) sc.wizard.steps.codeStep.errors.showCodeValidationError();
                                if (!nameValid) sc.wizard.steps.codeStep.errors.showNameValidationError();
                            }
                        }
                    }
                },
                stepDown: function () {
                    if (sc.wizard.position.currentPosition.codeStep) {
                        sc.wizard.position.setPosition.gotoPhoneStep();
                    }
                    else if (sc.wizard.position.currentPosition.passwordStep) {
                        sc.wizard.position.setPosition.gotoPhoneStep();
                    }
                }
            }
        };
    }
})();