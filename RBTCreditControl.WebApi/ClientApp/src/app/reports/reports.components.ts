import { Component, OnInit } from '@angular/core';
import { Http } from '@angular/http';
import { ActivatedRoute, Router } from '@angular/router';
import { GlobalSettings } from './../Services/GlobalSettings';
import { GridPagination } from './../Services/GlobalSettings'
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker/bs-datepicker.config';
//import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { NgxSpinnerService } from 'ngx-spinner';

declare var $: any;
@Component({
  templateUrl: './reports.component.html',
  providers: [GlobalSettings]
})
export class ReportsComponent implements OnInit {
  datePickerConfig: Partial<BsDatepickerConfig>;
  //dateRangePickerConfig: Partial<sDaterangepickerConfig>;


  myDateValue: Date;
  sessionToken: any = [];
  reportsData: any = [];

  supervisorReportsData: any = {

  };
  StatusData: any = [];
  statusSelected: any = [];
  supervisorReportsObj: any = {};
  successMsg = "";
  reportObj: any = [];
  //paging: any = [];
  storePage: any = [];
  role: any = [];
  paging = new GridPagination();
  range: any = [];


  constructor(private spinner: NgxSpinnerService, private _http: Http, private _globalService: GlobalSettings, private _route: ActivatedRoute, private _router: Router) {
    this.datePickerConfig = Object.assign({}, {
      containerClass: 'theme-dark-blue',
      showWeekNumbers: false,
      dateInputFormat: "DD/MM/YYYY"
    });
    //this.dateRangePickerConfig = Object.assign({}, {
    //  containerClass: 'theme-dark-blue',
    //  showWeekNumbers: false,
    //  dateInputFormat: "DD/MM/YYYY"
    //});
    this.datePickerConfig
    this.paging = this._globalService.paginationData;
  }


//  abcd() {
//  this.spinner.show();

//  setTimeout(() => {
//    /** spinner ends after 5 seconds */
//    this.spinner.hide();
//  }, 5000);
//}


  ngOnInit() {
    debugger;
    
    this.myDateValue = new Date();
    this.supervisorReportsObj.statusId = 0;
    this.supervisorReportsObj.userId = 0;
    if (localStorage.getItem('key') != null) {
      this.sessionToken = localStorage.getItem('key');
    }
    this.role = localStorage.getItem("role");
    if (this._route.snapshot.queryParams.key != this.sessionToken) {
      //this._router.navigate(['/home']);
      window.location.href = this._globalService.hostPath;
    }
    else {
      //this.getStatusDrpAPICall();
      this._http.get(this._globalService.hostPath + '/api/CorporateStatus/GetStausForReports?key=' + this.sessionToken)
        .subscribe(
          result => {
            debugger;
            console.log(result.json());
            var resultData = result.json();
            this.StatusData = resultData.statusList;

          }, error => {
            debugger;
            console.error(error);

          })
      if (localStorage.getItem("role") == 'Supervisor') {
        this._http.get(this._globalService.hostPath + '/api/User/GetUsersForSupervisor_ByLocation?key=' + this.sessionToken + '&userId=' + localStorage.getItem('id'))
          .subscribe(
            result => {
              debugger;
              console.log(result.json());
              var resultData = result.json();
              this.supervisorReportsData.userData = resultData.resp;

            }, error => {
              debugger;
              console.error(error);

            })
      }
    }
  }

  onDateChange(newDate: Date) {
    console.log(newDate);
  }
  getReportApiCall(pageNumber: number, pageSize: number) {
    debugger;
    //alert(this.range);
    debugger;
    this.supervisorReportsObj.Page = pageNumber;
    this.supervisorReportsObj.PageSize = pageSize;

    if (this.reportsData.fromDate == undefined || this.reportsData.toDate == undefined) {
      alert("Dates can't be empty");
      return false;
    }
    if (this.reportsData.fromDate> this.reportsData.toDate) {
        alert("From Date can't be greater than To Date");
        return false;   
    }
    this.spinner.show();
    this.supervisorReportsObj.dtFromDate = this.reportsData.fromDate.getMonth() + 1 + '/' + this.reportsData.fromDate.getDate() + '/' + this.reportsData.fromDate.getFullYear();
    this.supervisorReportsObj.dtToDate = this.reportsData.toDate.getMonth() + 1 + '/' + this.reportsData.toDate.getDate() + '/' + this.reportsData.toDate.getFullYear();

    debugger;
    this._http.post(this._globalService.hostPath + '/api/Report/UserCorprateReport?key=' + this.sessionToken, this.supervisorReportsObj).
        subscribe(result => {
          debugger;
          this.spinner.hide();
            var resultData = result.json();
            this.supervisorReportsData.resultData = resultData.resp.results;
            this.paging.totalRecords = resultData.resp.rowCount;
            this.paging.totalNoofpages = Math.ceil(this.paging.totalRecords / this.paging.paginationSize);

            this.storePage.pageNumber = resultData.resp.currentPage;
            this.storePage.pageSize = resultData.resp.pageSize;
            $("ul.pagination li").removeClass("active");
            $("ul.pagination #li" + pageNumber).addClass("active");
            $('#floatalert').fadeIn();
            if (resultData.status==400) {
                this.successMsg = resultData.msg;
                $('#floatalert').addClass('errormsg');
            }                
            //else {
            //    $('#floatalert').addClass('successmsg');
            //}  
        }, Error => {
            console.error('reports' + Error);
            $('#floatalert').addClass('errormsg');
            $('#floatalert').fadeIn();
            // this.successMsg = error.msg;                    
            this.successMsg = Error.msg;
        })
  }

  reportViewClicked(vData: any) {
    debugger;
    this.supervisorReportsData.viewCorporate = vData;
    this._http.get(this._globalService.hostPath + '/api/CorporateAction/GetStatusByCorpId?key=' + this.sessionToken + '&corpId=' + this.supervisorReportsData.viewCorporate.fK_SubmitedCorporateId)
      .subscribe(
        result => {
          debugger;
          console.log(result.json());
          var viewData = result.json();
          this.supervisorReportsData.viewCorporate = viewData.resp1[0];
          this.supervisorReportsData.corpStatusHistoryData = viewData.resp1[0].lstUserCorporateAction;
        }, error => {
          console.error(error);
        })
  }
  noofRecordstoDisplay(args: any) {
    this.paging.paginationSize = parseInt(args.target.value);
    this.paging.page = 1;// setting to default value
    this.paging.pageShow = 0;
    this.getReportApiCall(this.paging.page, this.paging.paginationSize);
  }
  currentPageBind(currPage: any) {
    this.paging.selectedIndex = currPage;
    this.paging.page = currPage;
    this.getReportApiCall(this.paging.page, this.paging.paginationSize);
    $("ul.pagination li").removeClass("active");
  }
  nextPage(nextpageNumber: number) {
    this.paging.page = nextpageNumber + 1;
    if (this.paging.page <= this.paging.totalNoofpages) {
      this.paging.pageShow = this.paging.pageShow + 10;
      this.getReportApiCall(this.paging.page, this.paging.paginationSize);
    }
  }
  previousPage(prevPageNumber: number) {
    if (prevPageNumber >= 0) {
      this.paging.page = prevPageNumber + 1;
      this.paging.pageShow = this.paging.pageShow - 10;
      this.getReportApiCall(this.paging.pageShow + 1, this.paging.paginationSize);
    }
  }
}
