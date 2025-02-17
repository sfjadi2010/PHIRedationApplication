import { TestBed } from '@angular/core/testing';

import { FileProcessService } from './file-process.service';

describe('FileProcessService', () => {
  let service: FileProcessService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FileProcessService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
