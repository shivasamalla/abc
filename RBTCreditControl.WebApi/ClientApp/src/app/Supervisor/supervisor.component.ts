import { Component, OnInit } from "@angular/core";
import { Http, RequestOptions } from '@angular/http';
import { GlobalSettings } from './../Services/GlobalSettings';
import { ActivatedRoute, Router } from "@angular/router";
import { NgxSpinnerService } from 'ngx-spinner';

declare var $: any;

@Component({
  templateUrl: "./supervisor.component.html",
  providers: [GlobalSettings]
})
export class SupervisorComponent implements OnInit {
  userData: any = [];
  drpdwnPageSizes = [50, 100, 200, 500];
  paginationSize = 50;
  page = 1;
  pageShow = 0;
  totalNoofpages: number = 0;
  totalRecords: number = 0;
  selectedIndex: number = 0;
  sessionToken: any;
  showUserCorpData: any = [];
  constructor(private spinner: NgxSpinnerService,private _http: Http, private _globalService: GlobalSettings, private _route: ActivatedRoute, private _router: Router) { }

  ngOnInit() {
    debugger;
    if (localStorage.getItem('key') != null) {
      this.sessionToken = localStorage.getItem('key');
    }
    if (this._route.snapshot.queryParams.key != this.sessionToken) {
      //this._router.navigate(['/home']);
      window.location.href = this._globalService.hostPath;
    }
    else {
      this.bindRBTUserList(this.page, this.paginationSize);
    }
  }
  bindRBTUserList(pageNumber: number, pageSize: number) {
    this.spinner.show();

    this._http.get(this._globalService.hostPath + '/api/User/GetALL_P?page=' + pageNumber + '&pagesize=' + pageSize + '&userId=' + localStorage.getItem('id') + '&key=' + this.sessionToken)
      .subscribe(result => {
        this.spinner.hide();
        console.log(result.json());
        var resultData = result.json();
        this.userData = resultData.resp.results;
        this.totalRecords = resultData.totalRecords;
        this.totalNoofpages = Math.ceil(this.totalRecords / this.paginationSize);
        $("ul.pagination li").removeClass("active");
        $("ul.pagination #li" + pageNumber).addClass("active");
      }, error => {
        this.spinner.hide();
        console.error();
      });
  }
  showUserCorporates(userId: any) {
    this.spinner.show();
    this._http.get(this._globalService.hostPath + '/api/Corporate/GetAssignedCorporate_ByUserId?key=' + this.sessionToken + '&userId=' + userId)
      .subscribe(result => {
        debugger;
        this.spinner.hide();
        this.showUserCorpData = result.json();
        console.log(result.json());
        //if (this.userDetails.length > 0) {
        //    for (var i = 0; i < this.userDetails.length; i++) {
        //        if (!this.checkedCorporateList.includes(this.userDetails[i].id)) {
        //            this.checkedCorporateList.push(this.userDetails[i].id)
        //        }
        //    }
        //}
        //if (this.checkedCorporateList.length > 0) {
        //    for (var i = 0; i < this.checkedCorporateList.length; i++) {
        //        $('#userCorpcheckbox_' + this.checkedCorporateList[i]).attr('checked', true);
        //    }
        //}

      }, error => {
        this.spinner.hide();
        console.error(error);
      });
  }

  changeUserStatus(currStatus: any) {
    debugger;
    this.spinner.show();
    if (confirm("Do you want to Change Staus !")) {
      this._http.put(this._globalService.hostPath + '/api/User/ActiveInActiveUser?key=' + this.sessionToken + '&id=' + currStatus.id + '&Flag=' + !currStatus.isActive, null)
        .subscribe(result => {
          debugger;
          this.spinner.hide();
          console.log('status' + result.json());
          if (result.status == 200 && result.json().msg == "Status Updated Successfully..") {
            currStatus.isActive = !currStatus.isActive;//making false in UI
          }
        }, error => {
          this.spinner.hide();
          console.error(error);
        })
    }
  }

  noofRecordstoDisplay(args: any) {
    this.paginationSize = parseInt(args.target.value);
    this.page = 1;
    this.pageShow = 0;
    this.bindRBTUserList(this.page, this.paginationSize);
  }
  currentPageBind(currPage: any) {
    this.selectedIndex = currPage;
    this.page = currPage;
    this.bindRBTUserList(currPage, this.paginationSize);
    $("ul.pagination li").removeClass("active");
  }
  nextPage(nextpageNumber: number) {
    this.page = nextpageNumber + 1;
    if (this.page <= this.totalNoofpages) {
      this.pageShow = this.pageShow + 10;
      this.bindRBTUserList(this.page, this.paginationSize);
    }
  }
  previousPage(prevPageNumber: number) {
    this.page = prevPageNumber + 1;
    if (prevPageNumber >= 0) {
      this.pageShow = this.pageShow - 10;
      this.bindRBTUserList(this.pageShow + 1, this.paginationSize);
    }
  }
}
