import { Component, OnInit } from '@angular/core';
import { Http, Headers, Response, RequestOptions } from '@angular/http';
import { GlobalSettings } from '../../../Services/GlobalSettings';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

declare var $: any;

@Component({
  templateUrl: './create-corporates.component.html'
})

export class CreateCorporatesComponent implements OnInit {
  searchKey: any;
  SupervisorLocation: any;
  sessionToken: any;
  customerData: any;
  successMsg = '';
  SupervisorLocationData: any = [];
  supervisorLocation: any = [];

  constructor(private spinner: NgxSpinnerService, private toastr: ToastrService, private _http: Http, private _globalService: GlobalSettings, private _route: ActivatedRoute, private _router: Router) { }
  ngOnInit() {
    if (localStorage.getItem('key') != null) {
      this.sessionToken = localStorage.getItem('key');
    }
    let urlKey = this._route.snapshot.queryParams.key;
    debugger;
    if (urlKey != this.sessionToken) {
      window.location.href = this._globalService.hostPath;
    }
    this.supervisorLocation = null;

    if (localStorage.getItem('id') != null) {
      let id = localStorage.getItem('id');
      
      this._http.get(this._globalService.hostPath + '/api/LocationMaster/GetByUserId?key=' + this.sessionToken + '&id=' + id)
        .subscribe(result => {
          debugger;
          this.spinner.hide();
          this.SupervisorLocationData = result.json().locationMaster;
          console.log(this.SupervisorLocationData);
        }, error => {
          this.spinner.hide();
          console.error(error);
        });
    }
    else {
      alert('Error in getting User Location Data')
    }


    //http://erp.riya.travel/erpvisaservice/services/GetCustomers?CompanyName=Riya%20Travel%20and%20Tours&LocationCode=BOM&SearchPattern=9764340
  }

  addCorporate(cust: any) {
    cust.CreatedBy = localStorage.getItem('id');
    for (var i = 0; i < this.SupervisorLocationData.length; i++) {
      if (this.supervisorLocation == this.SupervisorLocationData[i].branch) {
        cust.fk_locationId = this.SupervisorLocationData[i].id;
        break;
      }
    }
    this.spinner.show();
    debugger;
    this._http.post(this._globalService.hostPath + '/api/Corporate/InsertCorporate?key=' + this.sessionToken, cust)
      .subscribe(result => {
        this.spinner.hide();
        debugger;
        //this.customerData = result.json();
        console.log(result.json());
        this.successMsg = "Corporate Added  Succesfully";
        this.toastr.success(this.successMsg);


      }, error => {
        debugger;
        this.spinner.hide();
        this.successMsg = error.json().msg;
        this.toastr.error(this.successMsg);
        console.error(error);
      });    
  }
  GetICust() {
    debugger;
    this.spinner.show();
    //http://erp.riya.travel/erpvisaservice/services/GetCustomers?CompanyName=Riya%20Travel%20and%20Tours&LocationCode=
    this._http.get(this._globalService.hostPath + '/api/ERP/GetCorporate?Location=' + this.supervisorLocation + '&Icust=' + this.searchKey + '&key=' + this.sessionToken)
      .subscribe(result => {
        debugger;
        this.spinner.hide();
        $('#erpCorporateList').css('display', 'block');
        this.customerData = result.json();
        console.log(result.json());
      }, error => {
        debugger;
        this.spinner.hide();
        console.error(error);
      })
  }
}
