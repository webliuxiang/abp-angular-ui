import { EventEmitter, Injectable, Output } from '@angular/core';
import { Subject } from 'rxjs/Subject';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class DataAnalyzeService {

  constructor() {}
  @Output() getMetricsList = new EventEmitter();

  sub = new Subject<any>();
  // @Output() setMetricsList: EventEmitter<boolean> = new EventEmitter();

  metricsList: any;

  // editStatus
  private subjectEditStatus = new Subject<any>();
  // chartMessage
  private subjectChartMessage = new Subject<any>();
  // queryMessage
  private subjectQueryMessage = new Subject<any>();
  // aggMetricsMessage
  private subjectAggMetricsMessage = new Subject<any>();
  // aggBucketsMessage
  private subjectAggBucketsMessage = new Subject<any>();
  // select keymapping Message
  private subjectSelectKeysMessage = new Subject<any>();

  setMetricsList(e) {
    // console.log(e);
    this.metricsList = e;
    this.getMetricsList.emit(this.metricsList);
  }
  sendEditStatus(message: any) {
    this.subjectEditStatus.next(message);
  }

  getEditStatus(): Observable<any> {
    return this.subjectEditStatus.asObservable();
  }
  sendChartMessage(message: any) {
    this.subjectChartMessage.next(message);
  }

  getChartMessage(): Observable<any> {
    return this.subjectChartMessage.asObservable();
  }
  sendQueryMessage(message: any) {
    this.subjectQueryMessage.next(message);
  }

  getQueryMessage(): Observable<any> {
    return this.subjectQueryMessage.asObservable();
  }
  sendAggMetricsMessage(message: any) {
    this.subjectAggMetricsMessage.next(message);
  }

  getAggMetricsMessage(): Observable<any> {
    return this.subjectAggMetricsMessage.asObservable();
  }
  sendAggBucketsMessage(message: any) {
    this.subjectAggBucketsMessage.next(message);
  }

  getAggBucketsMessage(): Observable<any> {
    return this.subjectAggBucketsMessage.asObservable();
  }
  sendSelectKeysMessage(message: any) {
    this.subjectSelectKeysMessage.next(message);
  }

  getSelectKeysMessage(): Observable<any> {
    return this.subjectSelectKeysMessage.asObservable();
  }
}
