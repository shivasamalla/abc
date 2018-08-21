import { Injectable } from '@angular/core';
import { RequestOptions, Http } from '@angular/http';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker/bs-datepicker.config';
@Injectable()
export class GlobalSettings {
  //hostPath = "http://localhost:58412";
 
    hostPath = window.location.origin;
  // hostPath = "http://192.168.60.140:88";
    
  _datePickerConfig: Partial<BsDatepickerConfig>;
  paginationData = new GridPagination();
  _editorConfigData = {
    editable: true,
    spellcheck: true,
    height: '10rem',
    minHeight: '5rem',
    placeholder: 'Enter text here...',
    translate: 'no'
  };
  constructor() {
    this._datePickerConfig = Object.assign({}, {
      containerClass: 'theme-dark-blue',
      showWeekNumbers: false,
      dateInputFormat: "DD/MM/YYYY"
    });
  }
 
}
export interface IPagination {
  drpdwnPageSizes: any;
  paginationSize: number | undefined;
  page: number | undefined;
  pageShow: number | undefined;
  totalNoofpages: number | undefined;
  totalRecords: number | undefined;
  selectedIndex: number | undefined;
}
export class GridPagination implements IPagination {
  drpdwnPageSizes = [50, 100, 200, 500];
  paginationSize = 50;
  page = 1;
  pageShow = 0;
  totalNoofpages = 0;
  totalRecords = 0;
  selectedIndex = 0;
}
