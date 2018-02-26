(function () {
    'use strict';
    angular
        .module("angle", [
            'app.custom',
            'app.core',
            'lang.translate',
            'app.routes',
            'app.sidebar',
            'app.loadingbar',
            'app.translate',
            'app.icons',
            'app.pages',
            'oitozero.ngSweetAlert',
            'app.bootstrapui',
            'app.elements',
            'app.panels',
            'app.charts',
            'app.tables',
            'app.extras',
            'app.userauth',
            'toaster',
            'app.wizard',
            'app.layout',
            'app.global.map.location.service',
            'app.login.service',
            'app.cookie.service',
            'app.validator.service',
            'app.timer.service',
            'app.pagination.service',
            'app.rialto',
            'app.global.parlour.controller',
            'app.stocks.and.news.controller',
            'app.user.info.controller',
            'app.user.portfolio.controller',
            'app.user.task.controller',
            'app.user.executes.controller',
            'app.user.group.controller',
            'app.user.money.controller',
            'app.user.favorites.controller',
            'app.user.reviews.controller',
            'app.user.examination.controller',
            'app.user.awards.controller',
            'app.global.home.page.controller',
            'app.scroll.home.page',
            'app.modal.window.open.service',
            'app.sizeAndPagination.service'
        ]);
})();