import { Component, OnInit } from '@angular/core';
import { Http } from '@angular/http';
import { ActivatedRoute, Router, Data } from '@angular/router';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker/bs-datepicker.config';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { GlobalSettings } from '../Services/GlobalSettings';


declare var element: any;
declare var angular: any;
declare var $: any;

@Component({
  templateUrl: './user-corporates.component.html'
})
export class UserCorporatesComponent implements OnInit {
  datePickerConfig: Partial<BsDatepickerConfig>;
  sessionToken: any;
  successMsg: any = [];
  role: any = [];
  corporatesData: any = [];
  drpdwnPageSizes = [50, 100, 200, 500];
  paginationSize = 50;
  page = 1;
  pageShow = 0;
  totalNoofpages: number = 0;
  totalRecords: number = 0;
  selectedIndex: number = 0;
  selectedCorporatedData: any = [];
  storePage: any = [];
  Amount: any = [];
  userCorpDataObj: any = [];


  constructor(private spinner: NgxSpinnerService,private _http: Http, private _globalService: GlobalSettings, private _route: ActivatedRoute, private _router: Router, private toastr: ToastrService) {
    this.datePickerConfig = Object.assign({}, {
      containerClass: 'theme-dark-blue',
      showWeekNumbers: false,
      dateInputFormat: "DD/MM/YYYY"
    });
  }

  showSuccess() {
    this.toastr.success(this.successMsg);    
  }

  showError() {
    this.toastr.error(this.successMsg);
  }

  ngOnInit() {
    this.sessionToken = localStorage.getItem('key');
    this.role = localStorage.getItem('role');
    if (this._route.snapshot.queryParams.key == this.sessionToken) {
      this.bindUserCorporatesList(this.page, this.paginationSize)
    } else {
      //this._router.navigate(['/home']);
      window.location.href = this._globalService.hostPath;
    }

  }
  //ngAfterViewChecked() {
  //  $(".datepicker").datepicker({
  //    dateFormat: "dd-mm-yy"
  //  });
  //  //$(document).ready(function () {
  //  //    $('[data-toggle="tooltip"]').tooltip();
  //  //}); 
  //}
  bindUserCorporatesList(pageNumber: number, pageSize: number) {
    console.log(Date.now());
    this.storePage.pageNumber = pageNumber;
    this.storePage.pageSize = pageSize;

    this.spinner.show();

    this._http.get(this._globalService.hostPath + '/api/Corporate/GetUserAssignedCorporates?key=' + this.sessionToken + '&userId=' + localStorage.getItem('id') + '&page=' + pageNumber + ' &pagesize=' + pageSize)
      .subscribe(
      result => {
        
        this.spinner.hide();

  //      setTimeout(() => {
  //  /** spinner ends after 5 seconds */
  //  this.spinner.hide();
  //}, 5000);

          console.log('resp' + Date.now());
          //$('#progressBar').css('display', 'none');
          console.log(result.json());
          var resultData = result.json();
          this.corporatesData = resultData.results;
          this.totalRecords = resultData.totalRecords;
          this.totalNoofpages = Math.ceil(this.totalRecords / this.paginationSize);
          $("ul.pagination li").removeClass("active");
          $("ul.pagination #li" + pageNumber).addClass("active");


      }, error => {
        this.spinner.hide();

          console.error(error);
        })
  }
  corporateClicked(corp: any) {
    this.selectedCorporatedData = corp;
  }
  submitCorporate() {
    debugger;
    if (this.userCorpDataObj.fromDate > this.userCorpDataObj.toDate) {
      alert("From Date can't be greater than ToDate");
      return false;
    }
    var corpObj = {
      SbmtFromDate: this.userCorpDataObj.fromDate.getMonth() + 1 + '/' + this.userCorpDataObj.fromDate.getDate() + '/' + this.userCorpDataObj.fromDate.getFullYear(),
      SbmtToDate: this.userCorpDataObj.toDate.getMonth() + 1 + '/' + this.userCorpDataObj.toDate.getDate() + '/' + this.userCorpDataObj.toDate.getFullYear(),
      SbmtAmount: this.userCorpDataObj.amount,
      //FK_CorporateId: this.selectedCorporatedData.id,
      FK_CorporateStatusMasterId: 9 // 9 means status submission 
    }

   // $('#progressBar').css('display', 'block');
    this._http.post(this._globalService.hostPath + '/api/CorporateAction/PostStatus?Id=' + this.selectedCorporatedData.id + '&key=' + this.sessionToken, corpObj)
      .subscribe(result => {
       // $('#progressBar').css('display', 'none');

        console.log(result.json());
        this.bindUserCorporatesList(this.storePage.pageNumber, this.storePage.pageSize);
        //  this.successMsg = "Corporate Submitted Successfully";  
        var popupClose = document.getElementById("popupClose") as any;
        popupClose.click();

        this.successMsg = result.json().msg;
        if (result.json().status == true) {
          this.showSuccess();
        } else {
          this.showError();
        }
        this.userCorpDataObj = {};

      }, error => {
       // $('#progressBar').css('display', 'none');
        console.error(error);
        this.userCorpDataObj = {};
        this.successMsg = error.msg;
        this.showError();
        //  this.successMsg = "Error in Submitting Corporate";  
        var popupClose = document.getElementById("popupClose") as any;
        popupClose.click();
      })   
  }
  setPriority(selectedCorporate: any) {
   // $('#progressBar').css('display', 'block');

    //this.ngProgress.start();       
    //if (selectedCorporate.priority == 1) {
    //    selectedCorporate.priority = 0;
    //}
    //else {
    //    selectedCorporate.priority = 1;
    //}
    console.log('req' + Date.now());
    this._http.post(this._globalService.hostPath + '/api/Priority/SetPriority?key=' + this.sessionToken + '&priority=' + selectedCorporate.priority + '&CorporateId=' + selectedCorporate.id, null)
      .subscribe(result => {
        //this.ngProgress.done();
       // $('#progressBar').css('display', 'none');
        console.log('resp' + Date.now());
        var priorityResult = result.json();
        this.successMsg = priorityResult.msg;
       
        selectedCorporate.priority = priorityResult.priority;
        //if (selectedCorporate.priority == 1) {
        //    $('#' + selectedCorporate.no_).text('Low')
        //}
        //else {
        //    $('#' + selectedCorporate.no_).text('High')
        //}
        if (priorityResult.status == true) {
          this.showSuccess();
        }
        else {
          this.showError();
        }

      }, error => {
      //  $('#progressBar').css('display', 'none');
        console.error(error);        
        //  this.successMsg = "Error in Submitting Corporate";  
        this.successMsg = error.msg;
        //this.ngProgress.done();
        this.showError();
      })
  }
  RefreshGrid() {
    this.spinner.show();
   // $('#progressBar').css('display', 'block');
    //this.ngProgress.start();

    this.bindUserCorporatesList(this.storePage.pageNumber, this.storePage.pageSize)
  }

  noofRecordstoDisplay(args: any) {
    this.paginationSize = parseInt(args.target.value);
    this.page = 1;// setting to default value
    this.pageShow = 0;
    this.bindUserCorporatesList(this.page, this.paginationSize);
  }
  currentPageBind(currPage: any) {
    this.selectedIndex = currPage;
    this.page = currPage;
    this.bindUserCorporatesList(currPage, this.paginationSize);
    $("ul.pagination li").removeClass("active");
  }
  nextPage(nextpageNumber: number) {
    this.page = nextpageNumber + 1;
    if (this.page <= this.totalNoofpages) {
      this.pageShow = this.pageShow + 10;
      this.bindUserCorporatesList(this.page, this.paginationSize);
    }
  }
  previousPage(prevPageNumber: number) {
    if (prevPageNumber >= 0) {
      this.page = prevPageNumber + 1;
      this.pageShow = this.pageShow - 10;
      this.bindUserCorporatesList(this.pageShow + 1, this.paginationSize);
    }
  }
}
