"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var home_component_1 = require("./home/home.component");
var admin_component_1 = require("./Admin/admin.component");
var location_component_1 = require("./location/location.component");
var user_loggin_component_1 = require("./UserLogInUI/user-loggin.component");
var appRoutes = [
    {
        path: 'user', component: user_loggin_component_1.UserLoggIn,
        children: [
            { path: 'admin', component: admin_component_1.AdminComponent },
            { path: 'location', component: location_component_1.LocationComponent }
        ]
    },
    { path: '', component: home_component_1.HomeComponent, pathMatch: 'full' }
];
//# sourceMappingURL=app-routing.module.js.map