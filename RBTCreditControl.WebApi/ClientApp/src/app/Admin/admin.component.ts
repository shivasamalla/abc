import { Component, OnInit } from "@angular/core";
import { Http } from '@angular/http';
import { GlobalSettings } from './../Services/GlobalSettings';
import { ActivatedRoute, Router } from "@angular/router";

declare var $: any;

@Component({
  templateUrl: "./admin.component.html",
  providers: [GlobalSettings]
})
export class AdminComponent implements OnInit {
  userData: any = [];
  drpdwnPageSizes = [50, 100, 500];
  paginationSize = 50;
  page = 1;
  pageShow = 0;
  totalNoofpages: number = 0;
  totalRecords: number = 0;
  sessionToken: any;
  constructor(private _http: Http, private _globalService: GlobalSettings, private _Route: ActivatedRoute, private _router: Router) { }

  ngOnInit() {
    if (localStorage.getItem('key') != null) {
      this.sessionToken = localStorage.getItem('key');
    }
    let urlKey = this._Route.snapshot.queryParams.key;
    debugger;
    if (urlKey != this.sessionToken) {
      window.location.href = this._globalService.hostPath;
    }
    this.bindRBTUserList(this.page, this.paginationSize);
  }
  bindRBTUserList(pageNumber: number, pageSize: number) {
    this._http.get(this._globalService.hostPath + '/api/RBTAdmin/GetAllUsers?page=' + pageNumber + '&pagesize=' + pageSize + '&key=' + this.sessionToken)
      .subscribe(result => {
        debugger;
        console.log(result.json());
        var resultData = result.json();
        this.userData = resultData.resp.results;
        this.totalRecords = resultData.totalRecords;
        this.totalNoofpages = Math.ceil(this.totalRecords / this.paginationSize);
      }, error => {
        console.error();
      });
  }
  //showCorporates(id: number) {
  //    $('#corporates_' + id).css("display", "block");
  //}
  noofRecordstoDisplay(args: any) {
    this.paginationSize = parseInt(args.target.value);
    this.page = 1;
    this.pageShow = 0;
    this.bindRBTUserList(this.page, this.paginationSize);
  }
  currentPageBind(currPage: any) {
    this.page = currPage;
    this.bindRBTUserList(currPage, this.paginationSize);
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
    if (prevPageNumber <= this.totalNoofpages) {
      this.pageShow = this.pageShow - 10;
      this.bindRBTUserList(this.pageShow + 1, this.paginationSize);
    }
  }

}
