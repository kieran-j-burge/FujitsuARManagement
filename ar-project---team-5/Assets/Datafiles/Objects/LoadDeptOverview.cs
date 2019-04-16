﻿using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class LoadDeptOverview : MonoBehaviour, ITrackableEventHandler
{
    public LoadDeptOverview()
    {
        Credentials = new Credentials("cristiano.bellucci.fujitsu+cardiffadmin@gmail.com", "Millennium");
    }

    private TrackableBehaviour _trackableBehaviour;

    private ApiHelper ApiHelper { get; set; }

    private Credentials Credentials { get; set; }

    public GameObject ProjectRow;

    private bool UIUpdated = false;

    public void Start()
    {
        _trackableBehaviour = GetComponent<TrackableBehaviour>();

        if (_trackableBehaviour)
        {
            _trackableBehaviour.RegisterTrackableEventHandler(this);
        }
    }

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED || newStatus == TrackableBehaviour.Status.TRACKED || newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            //Image target is found

            if (ApiHelper == null)
            {
                ApiHelper = new ApiHelper(Credentials.Username, Credentials.Password);
            }

            switch (_trackableBehaviour.name)
            {
                case "HR":
                    {
                        AsyncWorker<DepartmentOverviewData>.Dispatch((worker) =>
                        {
                            Feed<OrganisationalUnit> organisationalUnits = ApiHelper.GetOrganisationalUnits();

                            Feed<OrganisationalUnitProcessType> organisationalUnitProcessTypes = ApiHelper.GetOrganisationalUnitProcessTypes();

                            Debug.Log("HR");

                            foreach (var a in organisationalUnitProcessTypes.Entries)
                            {
                                Debug.Log(a.Title);
                            }


                            Feed<OrganisationalUnitProcess> organisationalUnitProcesses1 = ApiHelper.GetOrganisationalUnitProcesses(
                                organisationalUnitProcessTypes.Entries
                                    .Where((organisationalUnitProcessType) => organisationalUnitProcessType.Title.ToLower() == "cwl hr offboarding" )
                                    .FirstOrDefault()
                                    .Id);

                            Feed<OrganisationalUnitProcess> organisationalUnitProcesses2 = ApiHelper.GetOrganisationalUnitProcesses(
                               organisationalUnitProcessTypes.Entries
                                   .Where((organisationalUnitProcessType) =>  organisationalUnitProcessType.Title.ToLower() == "cwl hr onboarding")
                                   .FirstOrDefault()
                                   .Id);

                           

                            List<ProcessStatuses> processStatuses1 = organisationalUnitProcesses1.Entries
                                .Select((organisationalUnitProcess) =>
                                {
                                    return (ProcessStatuses)Convert.ToInt32(organisationalUnitProcess.Categories
                                        .Where((organisationalUnitProcessCategory) => organisationalUnitProcessCategory.Term.ToLower() == "status")
                                        .FirstOrDefault()
                                        .Label);
                                })
                                .ToList();

                            List<ProcessStatuses> processStatuses2 = organisationalUnitProcesses2.Entries
                                .Select((organisationalUnitProcess) =>
                                {
                                    return (ProcessStatuses)Convert.ToInt32(organisationalUnitProcess.Categories
                                        .Where((organisationalUnitProcessCategory) => organisationalUnitProcessCategory.Term.ToLower() == "status")
                                        .FirstOrDefault()
                                        .Label);
                                })
                                .ToList();

                            int Process1Error = processStatuses1
                                    .Where((processStatus) => processStatus == ProcessStatuses.TechnicalError)
                                    .Count();

                            int Process2Error = processStatuses2
                                    .Where((processStatus) => processStatus == ProcessStatuses.TechnicalError)
                                    .Count();

                            int[] InstancesInError = { Process1Error,Process2Error};


                            DepartmentOverviewData deptData = new DepartmentOverviewData()
                            {
                                OrganisationalUnitProcessType = organisationalUnitProcessTypes,
                                InstanceCount = InstancesInError
                            };

                            //worker.ReportProgress(new OrganisationalUnitStatuses(
                            //    processStatuses
                            //        .Where((processStatus) => processStatus == ProcessStatuses.TechnicalError)
                            //        .Count(),
                            //    processStatuses
                            //        .Where((processStatus) => processStatus == ProcessStatuses.Ok)
                            //        .Count(),
                            //    processStatuses
                            //        .Where((processStatus) => processStatus == ProcessStatuses.Awaiting)
                            //        .Count()));

                            worker.ReportProgress(deptData);


                        }, (callback) =>
                        {
                            if (UIUpdated == false)
                            {
                                AddProjects("HR", callback);
                                UIUpdated = true;
                            }
                            else {
                            }
                            //Do whatever with the values here

                        }).RunAsync();

                        break;
                    }

                case "Marketing":
                    {
                        AsyncWorker<DepartmentOverviewData>.Dispatch((worker) =>
                        {
                            Feed<OrganisationalUnit> organisationalUnits = ApiHelper.GetOrganisationalUnits();

                            Feed<OrganisationalUnitProcessType> organisationalUnitProcessTypes = ApiHelper.GetOrganisationalUnitProcessTypes();

                            Debug.Log("Marketing");

                            foreach (var a in organisationalUnitProcessTypes.Entries)
                            {
                                Debug.Log(a.Title);
                            }

                            Feed<OrganisationalUnitProcess> organisationalUnitProcesses1 = ApiHelper.GetMarketingFairOrganisationalUnitProcesses();

                            Feed<OrganisationalUnitProcess> organisationalUnitProcesses2 = ApiHelper.GetMarketingCampaignOrganisationalUnitProcesses();

                            List<ProcessStatuses> processStatuses1 = organisationalUnitProcesses1.Entries
                                .Select((organisationalUnitProcess) =>
                                {
                                    return (ProcessStatuses)Convert.ToInt32(organisationalUnitProcess.Categories
                                        .Where((organisationalUnitProcessCategory) => organisationalUnitProcessCategory.Term.ToLower() == "status")
                                        .FirstOrDefault()
                                        .Label);
                                })
                                .ToList();

                            List<ProcessStatuses> processStatuses2 = organisationalUnitProcesses2.Entries
                                .Select((organisationalUnitProcess) =>
                                {
                                    return (ProcessStatuses)Convert.ToInt32(organisationalUnitProcess.Categories
                                        .Where((organisationalUnitProcessCategory) => organisationalUnitProcessCategory.Term.ToLower() == "status")
                                        .FirstOrDefault()
                                        .Label);
                                })
                                .ToList();

                            int Process1Error = processStatuses1
                                    .Where((processStatus) => processStatus == ProcessStatuses.TechnicalError)
                                    .Count();

                            int Process2Error = processStatuses2
                                    .Where((processStatus) => processStatus == ProcessStatuses.TechnicalError)
                                    .Count();

                            int[] InstancesInError = { Process1Error, Process2Error };


                            DepartmentOverviewData deptData = new DepartmentOverviewData()
                            {
                                OrganisationalUnitProcessType = organisationalUnitProcessTypes,
                                InstanceCount = InstancesInError
                            };

                            //worker.ReportProgress(new OrganisationalUnitStatuses(
                            //    processStatuses
                            //        .Where((processStatus) => processStatus == ProcessStatuses.TechnicalError)
                            //        .Count(),
                            //    processStatuses
                            //        .Where((processStatus) => processStatus == ProcessStatuses.Ok)
                            //        .Count(),
                            //    processStatuses
                            //        .Where((processStatus) => processStatus == ProcessStatuses.Awaiting)
                            //        .Count()));

                            worker.ReportProgress(deptData);


                        }, (callback) =>
                        {
                            if (UIUpdated == false)
                            {
                                AddProjects("Marketing", callback);
                                UIUpdated = true;
                            }
                            else
                            {
                            }
                            //Do whatever with the values here

                        }).RunAsync();

                        break;
                    }

                case "Engineering":
                    {
                        AsyncWorker<DepartmentOverviewData>.Dispatch((worker) =>
                        {
                            Feed<OrganisationalUnit> organisationalUnits = ApiHelper.GetOrganisationalUnits();

                            Feed<OrganisationalUnitProcessType> organisationalUnitProcessTypes = ApiHelper.GetOrganisationalUnitProcessTypes();

                            Debug.Log("Engineering");

                            foreach (var a in organisationalUnitProcessTypes.Entries)
                            {
                                Debug.Log(a.Title);
                            }
                            

                            Feed<OrganisationalUnitProcess> organisationalUnitProcesses1 = ApiHelper.GetEngineeringProjectOrganisationalUnitProcesses();

                            Feed<OrganisationalUnitProcess> organisationalUnitProcesses2 = ApiHelper.GetEngineeringReleaseOrganisationalUnitProcesses();

                            Feed<OrganisationalUnitProcess> organisationalUnitProcesses3 = ApiHelper.GetEngineeringTestOrganisationalUnitProcesses();




                            List<ProcessStatuses> processStatuses1 = organisationalUnitProcesses1.Entries
                                .Select((organisationalUnitProcess) =>
                                {
                                    return (ProcessStatuses)Convert.ToInt32(organisationalUnitProcess.Categories
                                        .Where((organisationalUnitProcessCategory) => organisationalUnitProcessCategory.Term.ToLower() == "status")
                                        .FirstOrDefault()
                                        .Label);
                                })
                                .ToList();

                            List<ProcessStatuses> processStatuses2 = organisationalUnitProcesses2.Entries
                                .Select((organisationalUnitProcess) =>
                                {
                                    return (ProcessStatuses)Convert.ToInt32(organisationalUnitProcess.Categories
                                        .Where((organisationalUnitProcessCategory) => organisationalUnitProcessCategory.Term.ToLower() == "status")
                                        .FirstOrDefault()
                                        .Label);
                                })
                                .ToList();

                            List<ProcessStatuses> processStatuses3 = organisationalUnitProcesses3.Entries
                                .Select((organisationalUnitProcess) =>
                                {
                                    return (ProcessStatuses)Convert.ToInt32(organisationalUnitProcess.Categories
                                        .Where((organisationalUnitProcessCategory) => organisationalUnitProcessCategory.Term.ToLower() == "status")
                                        .FirstOrDefault()
                                        .Label);
                                })
                                .ToList();

                            int Process1Error = processStatuses1
                                    .Where((processStatus) => processStatus == ProcessStatuses.TechnicalError)
                                    .Count();

                            int Process2Error = processStatuses2
                                    .Where((processStatus) => processStatus == ProcessStatuses.TechnicalError)
                                    .Count();

                            int Process3Error = processStatuses3
                                    .Where((processStatus) => processStatus == ProcessStatuses.TechnicalError)
                                    .Count();

                            Debug.Log(Process1Error + "   "+ Process2Error + "   " + Process3Error + "   ");

                            int[] InstancesInError = { Process1Error, Process2Error, Process3Error };


                            DepartmentOverviewData deptData = new DepartmentOverviewData()
                            {
                                OrganisationalUnitProcessType = organisationalUnitProcessTypes,
                                InstanceCount = InstancesInError
                            };

                            ////worker.ReportProgress(new OrganisationalUnitStatuses(
                            ////    processStatuses
                            ////        .Where((processStatus) => processStatus == ProcessStatuses.TechnicalError)
                            ////        .Count(),
                            ////    processStatuses
                            ////        .Where((processStatus) => processStatus == ProcessStatuses.Ok)
                            ////        .Count(),
                            ////    processStatuses
                            ////        .Where((processStatus) => processStatus == ProcessStatuses.Awaiting)
                            ////        .Count()));

                            worker.ReportProgress(deptData);


                        }, (callback) =>
                        {
                            if (UIUpdated == false)
                            {
                                AddProjects("Engineering", callback);
                                UIUpdated = true;
                            }
                            else
                            {
                            }
                            //Do whatever with the values here

                        }).RunAsync();

                        break;
                    }
            }
        }
    }


    private void AddProjects(String dept, DepartmentOverviewData callback) {

        GameObject deptObj = GameObject.Find(dept).gameObject;
        GameObject projectContainer = deptObj.transform.Find("DeptOverview").transform.Find("Middle Container").gameObject;

        if (dept == "Marketing") {
            dept = "Market";
        }

        else if (dept == "Engineering") {
            dept = "Eng";
        }

        int Count = 1;
        //int spacer = 60;
        foreach (var a in callback.OrganisationalUnitProcessType.Entries)
        {
            if (a.Title.Contains(dept))
            {
                //Set rows position 
                GameObject row = Instantiate(ProjectRow) as GameObject;
                row.transform.SetParent(projectContainer.transform);
                row.transform.localPosition = new Vector3(0, -(60 * Count), 0);
                row.transform.localRotation = Quaternion.identity;
                row.transform.localScale = new Vector3(1, 1, 1);

                //set row data
                row.transform.Find("project-name").GetComponent<Text>().text = a.Title;
                row.transform.Find("project-instances").GetComponent<Text>().text = callback.InstanceCount[Count - 1].ToString();

                if (callback.InstanceCount[Count - 1] > 0)
                {
                    deptObj.transform.Find("DeptOverview").transform.Find("Top Container").transform.Find("Dept Status").GetComponent<UnityEngine.UI.Image>().color = new Color32(220, 150, 0, 255);
                    row.transform.Find("project-status").GetComponent<UnityEngine.UI.Image>().color = new Color32(220, 150, 0, 255);
                }

                else if (callback.InstanceCount[Count - 1] == 0) {
                    row.transform.Find("project-status").GetComponent<UnityEngine.UI.Image>().color = new Color32(15,225,0,255);
                }
                
                Count = Count + 1;
            }

        }
        Count = 0;

    }
}